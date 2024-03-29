using Microsoft.EntityFrameworkCore;
using NewJobSurveyAdmin.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using NewJobSurveyAdmin.Services.CallWeb;

namespace NewJobSurveyAdmin.Services
{
    public class EmployeeCreationService
    {
        private CallWebService callWeb;
        private NewJobSurveyAdminContext context;
        private EmployeeInfoLookupService infoLookupService;
        private LoggingService logger;

        public EmployeeCreationService(
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

        public List<Employee> ExistingEmployees(string[] candidateGovernmentEmployeeIds)
        {
            return context.Employees
                .Where(e => candidateGovernmentEmployeeIds.Contains(e.GovernmentEmployeeId))
                .Include(e => e.CurrentEmployeeStatus)
                .ToList();
        }

        // NB. For reconciliation purposes, existence is determined by the
        // combination of EmployeeId, ExitCount, and record count.
        public Employee FindExisting(Employee candidate, List<Employee> employeesToSearch)
        {
            var employee = employeesToSearch.Find(
                e =>
                    e.GovernmentEmployeeId == candidate.GovernmentEmployeeId
                    && e.EffectiveDate == candidate.EffectiveDate
            );

            return employee;
        }

        public async Task<EmployeeTaskResult> InsertEmployees(List<Employee> employees)
        {
            var employeeTaskResult = new EmployeeTaskResult(TaskEnum.ReconcileEmployees);

            var dates = await SurveyDateBuilder.GetDatesBasedOnAdminSettings(context);

            var existingEmployees = ExistingEmployees(
                employees.Select(e => e.GovernmentEmployeeId).ToArray()
            );

            // Do this in a batch, working with 50 employees at a time.
            var BATCH_SIZE = 50;
            var NUM_BATCHES = (int)Math.Ceiling((double)employees.Count() / BATCH_SIZE);
            for (var i = 0; i < NUM_BATCHES; i++)
            {
                var employeesInBatch = employees.Skip(i * BATCH_SIZE).Take(BATCH_SIZE).ToList();

                // Step 1. Prepare employees.
                var employeesToCreate = employeeTaskResult.AddIncrementalStep(
                    PrepareEmployees(employeesInBatch, existingEmployees, dates)
                );

                // Step 2. Get telkeys.
                var employeesToSave = employeeTaskResult.AddIncrementalStep(
                    await callWeb.CreateSurveys(employeesToCreate)
                );

                // Step 3. Save context.
                var result = await SaveNewEmployees(employeesToSave);
                employeeTaskResult.AddFinalStep(result);
            }

            return employeeTaskResult;
        }

        private TaskResult<Employee> PrepareEmployees(
            List<Employee> employees,
            List<Employee> existingEmployees,
            SurveyDates dates
        )
        {
            var taskResult = new TaskResult<Employee>();

            foreach (var employee in employees)
            {
                // Check to see if an employee already exists.
                var existingEmployee = FindExisting(employee, existingEmployees);

                try
                {
                    if (existingEmployee != null)
                    {
                        // The unique user exists in the database. In this case,
                        // for NJSA, we throw an exception; we don't try to
                        // update user data.
                        throw new DuplicateEmployeeException(
                            $"Employee with ID {employee.GovernmentEmployeeId} and "
                                + $"hire date {employee.EffectiveDate.ToString("yyyy-MM-dd")} "
                                + "already exists."
                        );
                    }

                    // Otherwise, the employee does not exist in the database.

                    // Set the status code for a new employee.
                    var newStatusCode = EmployeeStatusEnum.Active.Code;
                    employee.CurrentEmployeeStatusCode = newStatusCode;

                    // Set info from LDAP. If no info is found, an exception will
                    // be thrown.
                    employee.UpdateInfoFromLdap(infoLookupService);

                    // Set other preferred fields; runs on creation only.
                    employee.InstantiateFields(dates);

                    // Ensure that the employee is valid.
                    var nullRequiredProperties = employee.NullRequiredProperties();

                    if (nullRequiredProperties.Count() > 0)
                    {
                        throw new InvalidOperationException(
                            "Employee has null properties that are required: "
                                + String.Join(",", nullRequiredProperties.Select(p => p.Name))
                        );
                    }

                    // Set timeline entries.
                    employee.TimelineEntries = new List<EmployeeTimelineEntry>();
                    employee.TimelineEntries.Add(
                        new EmployeeTimelineEntry
                        {
                            EmployeeActionCode = EmployeeActionEnum.CreateFromCSV.Code,
                            EmployeeStatusCode = newStatusCode,
                            Comment = "Created automatically by script."
                        }
                    );

                    taskResult.AddSucceeded(employee);
                }
                catch (Exception exception)
                {
                    taskResult.AddFailedWithException(
                        employee,
                        new FailedToPrepareEmployeeException(
                            $"Could not prepare employee {employee}: {exception.Message}"
                        )
                    );
                }
            }

            return taskResult;
        }

        private async Task<TaskResult<Employee>> SaveNewEmployees(List<Employee> employees)
        {
            var taskResult = new TaskResult<Employee>();

            try
            {
                foreach (var e in employees)
                {
                    context.Employees.Add(e);
                }
                await context.SaveChangesAsync();
                taskResult.AddSucceeded(employees);
            }
            catch (Exception exception)
            {
                // Remove the employee and the timeline entry from the
                // context, or else we will also get errors next time we
                // next try to save changes to the context.
                foreach (var employee in employees)
                {
                    context.RemoveRange(employee.TimelineEntries);
                    context.Remove(employee);
                }

                // Assume that saving all employees failed.
                taskResult.AddFailed(employees);
                taskResult.AddException(
                    new FailedToSaveContextException(
                        $"Saving employees failed for a range of employees: {String.Join(", ", employees)}. Error: {exception.Message}"
                    )
                );
            }

            return taskResult;
        }
    }
}
