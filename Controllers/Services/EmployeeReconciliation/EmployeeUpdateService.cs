using Microsoft.EntityFrameworkCore;
using NewJobSurveyAdmin.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using NewJobSurveyAdmin.Services.CallWeb;

namespace NewJobSurveyAdmin.Services
{
    public class EmployeeUpdateService
    {
        private CallWebService callWeb;
        private NewJobSurveyAdminContext context;
        private EmployeeInfoLookupService infoLookupService;
        private LoggingService logger;

        public EmployeeUpdateService(
            NewJobSurveyAdminContext context,
            CallWebService callWeb,
            EmployeeInfoLookupService infoLookupService,
            LoggingService logger
        )
        {
            this.context = context;
            this.callWeb = callWeb;
            this.infoLookupService = infoLookupService;
            this.logger = logger;
        }

        public async Task<EmployeeTaskResult> RefreshCallWebStatus()
        {
            var employeeTaskResult = new EmployeeTaskResult(TaskEnum.RefreshStatuses);

            // For all non-final employees, update.
            var employees = EmployeesNeedingCallWebRefresh();

            // Do this in a batch, working with 100 employees at a time.
            var BATCH_SIZE = 100;
            var NUM_BATCHES = (int)Math.Ceiling((double)employees.Count() / BATCH_SIZE);
            for (var i = 0; i < NUM_BATCHES; i++)
            {
                var employeesInBatch = employees.Skip(i * BATCH_SIZE).Take(BATCH_SIZE).ToList();

                // Step 1. Get the status codes for the employees.
                var employeesWithSurveyStatusCodes = employeeTaskResult.AddIncrementalStep(
                    await callWeb.GetSurveyStatusCodes(employeesInBatch)
                );

                // Step 2. Update the statuses.
                var result = await UpdateEmployeeSurveyStatuses(employeesWithSurveyStatusCodes);
                employeeTaskResult.AddFinalStep(result);
            }

            return employeeTaskResult;
        }

        private async Task<TaskResult<Employee>> UpdateEmployeeSurveyStatuses(
            List<Tuple<Employee, string>> employeeResultsWithSurveyStatusCodes
        )
        {
            var taskResult = new TaskResult<Employee>();

            var employeesToSave = new List<Tuple<Employee, EmployeeStatusEnum>>();

            foreach (var tuple in employeeResultsWithSurveyStatusCodes)
            {
                var employee = tuple.Item1;
                var callWebStatusCode = tuple.Item2;

                if (callWebStatusCode == null)
                {
                    // The employee does not have a valid status code.
                    taskResult.AddFailedWithException(
                        employee,
                        new NullCallWebStatusCodeException($"No status code for ${employee}")
                    );
                    continue;
                }

                // Check if the employee has completed the survey.
                if (callWebStatusCode.Equals(EmployeeStatusEnum.SurveyComplete.Code))
                {
                    employeesToSave.Add(Tuple.Create(employee, EmployeeStatusEnum.SurveyComplete));
                    continue;
                }

                // An employee only has a set amount of time to complete a survey.
                // If that time has expired, then expire the user.
                if (
                    employee.DeadlineDate.AddDays(1) <= DateTime.UtcNow
                    && employee.CurrentEmployeeStatusCode != EmployeeStatusEnum.Expired.Code
                )
                {
                    employeesToSave.Add(Tuple.Create(employee, EmployeeStatusEnum.Expired));
                    continue;
                }
            }

            taskResult.AddFinal(await SaveStatusesAndAddTimelineEntries(employeesToSave));

            return taskResult;
        }

        public async Task<TaskResult<Employee>> SaveStatusesAndAddTimelineEntries(
            List<Tuple<Employee, EmployeeStatusEnum>> employeesWithStatuses
        )
        {
            var taskResult = new TaskResult<Employee>();

            var employeesToUpdate = new List<Employee>();

            // First, update the status and add a timeline entry.
            foreach (var tuple in employeesWithStatuses)
            {
                var employee = tuple.Item1;
                var newStatus = tuple.Item2;

                var newStatusCode = newStatus.Code;
                var oldStatusCode = employee.CurrentEmployeeStatusCode;

                // Update employee status.
                employee.CurrentEmployeeStatusCode = newStatusCode;

                // Create a new timeline entry.
                employee.TimelineEntries.Add(
                    new EmployeeTimelineEntry
                    {
                        EmployeeActionCode = EmployeeActionEnum.UpdateByTask.Code,
                        EmployeeStatusCode = newStatusCode,
                        Comment =
                            $"Status updated by script: " + $"{oldStatusCode} → {newStatusCode}."
                    }
                );

                employeesToUpdate.Add(employee);
            }

            // Update in CallWeb.
            var employeesToSave = taskResult.AddIncremental(
                await callWeb.UpdateSurveys(employeesToUpdate)
            );

            taskResult.AddFinal(await SaveExistingEmployees(employeesToSave));

            return taskResult;
        }

        public async Task<TaskResult<Employee>> SaveExistingEmployees(List<Employee> employees)
        {
            var taskResult = new TaskResult<Employee>();

            try
            {
                employees.Select(e => context.Entry(e).State = EntityState.Modified);
                await context.SaveChangesAsync();
                taskResult.AddSucceeded(employees);
            }
            catch (Exception exception)
            {
                // Assume saving all employees failed.
                taskResult.AddFailed(employees);
                taskResult.AddException(
                    new FailedToSaveContextException(
                        $"Saving employees failed for a range of employees: {String.Join(", ", employees)}. Error: {exception.Message}"
                    )
                );
            }

            return taskResult;
        }

        private List<Employee> EmployeesNeedingCallWebRefresh()
        {
            return context.Employees
                .Include(e => e.TimelineEntries)
                .Include(e => e.CurrentEmployeeStatus)
                .Where(e => (e.CurrentEmployeeStatus.State != EmployeeStatusEnum.StateFinal))
                .ToList();
        }
    }
}
