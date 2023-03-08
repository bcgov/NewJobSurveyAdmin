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

        public async Task<Employee> SaveStatusAndAddTimelineEntry(
            Employee employee,
            EmployeeStatusEnum newStatus
        )
        {
            var newStatusCode = newStatus.Code;
            var oldStatusCode = employee.CurrentEmployeeStatusCode;

            // Update employee status.
            employee.CurrentEmployeeStatusCode = newStatusCode;

            // Create a new timeline entry.
            employee.TimelineEntries.Add(new EmployeeTimelineEntry
            {
                EmployeeActionCode = EmployeeActionEnum.UpdateByTask.Code,
                EmployeeStatusCode = newStatusCode,
                Comment = $"Status updated by script: " +
                          $"{oldStatusCode} → {newStatusCode}."
            });
            context.Entry(employee).State = EntityState.Modified;

            // Update in CallWeb.
            await callWeb.UpdateSurvey(employee);

            // Save.
            await context.SaveChangesAsync();

            return employee;
        }

        private async Task<Employee> UpdateEmployeeStatus(
            Employee employee
        )
        {
            var callWebStatusCode = await callWeb.GetSurveyStatusCode(employee);

            // Check if the employee has completed the survey.
            if (callWebStatusCode.Equals(EmployeeStatusEnum.SurveyComplete.Code))
            {
                return await SaveStatusAndAddTimelineEntry(employee,
                    EmployeeStatusEnum.SurveyComplete);
            }

            // An employee only has a set amount of time to complete a survey.
            // If that time has expired, then expire the user.
            if (
                employee.DeadlineDate.AddDays(1) <= DateTime.UtcNow &&
                employee.CurrentEmployeeStatusCode != EmployeeStatusEnum.Expired.Code
            )
            {
                return await SaveStatusAndAddTimelineEntry(employee,
                    EmployeeStatusEnum.Expired);
            }

            return employee;
        }

        public async Task<EmployeeTaskResult> UpdateEmployeeStatuses()
        {
            var updatedEmployeeList = new List<Employee>();
            var exceptionList = new List<string>();

            // For all non-final employees and expired employees, update.
            var candidateEmployees = context.Employees
                .Include(e => e.TimelineEntries)
                .Include(e => e.CurrentEmployeeStatus)
                .Where(
                    e => (e.CurrentEmployeeStatus.State != EmployeeStatusEnum.StateFinal) ||
                         (e.CurrentEmployeeStatusCode == EmployeeStatusEnum.Expired.Code)
                )
                .ToList();

            foreach (Employee e in candidateEmployees)
            {
                try
                {
                    var employee = await UpdateEmployeeStatus(e);
                    updatedEmployeeList.Add(employee);
                }
                catch (Exception exception)
                {
                    exceptionList.Add(
                        $"Exception updating status of employee {e.FullName} " +
                        $"(ID: {e.GovernmentEmployeeId}): {exception.GetType()}: {exception.Message} "
                    );
                }
            }

            return new EmployeeTaskResult(
                TaskEnum.RefreshStatuses,
                candidateEmployees.Count,
                updatedEmployeeList,
                exceptionList
            );
        }
    }
}
