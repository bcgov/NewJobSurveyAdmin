using NewJobSurveyAdmin.Models;
using NewJobSurveyAdmin.Services.CallWeb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin.Services
{
    public class EmployeeBlackoutPeriodService
    {
        private CallWebService callWeb;
        private NewJobSurveyAdminContext context;
        private LoggingService logger;
        private EmployeeCreationService insertionService;

        public EmployeeBlackoutPeriodService(
            NewJobSurveyAdminContext context,
            CallWebService callWeb,
            LoggingService logger,
            EmployeeCreationService insertionService
        )
        {
            this.context = context;
            this.callWeb = callWeb;
            this.logger = logger;
            this.insertionService = insertionService;
        }

        public async Task<Employee> UpdateBlackoutPeriod(
            Employee employee,
            SurveyDates dates
        )
        {
            // Update employee dates.
            employee.SetDates(dates);

            // Update in CallWeb.
            await callWeb.UpdateSurvey(employee);

            // Create a new timeline entry.
            employee.TimelineEntries.Add(new EmployeeTimelineEntry
            {
                EmployeeActionCode = EmployeeActionEnum.UpdateByTask.Code,
                EmployeeStatusCode = employee.CurrentEmployeeStatusCode,
                Comment = $"Blackout dates unset by script. New dates: " +
                          $"InviteDate → {dates.InviteDate.ToString("yyyy-MM-dd")}, " +
                          $"Reminder1Date → {dates.Reminder1Date.ToString("yyyy-MM-dd")}, " +
                          $"Reminder2Date → {dates.Reminder2Date.ToString("yyyy-MM-dd")}, " +
                          $"DeadlineDate → {dates.DeadlineDate.ToString("yyyy-MM-dd")}."
            });
            context.Entry(employee).State = EntityState.Modified;

            // Save.
            await context.SaveChangesAsync();

            return employee;
        }

        public async Task<EmployeeTaskResult> UpdateBlackoutPeriods()
        {
            var updatedEmployeeList = new List<Employee>();
            var exceptionList = new List<string>();

            var blackoutPeriodSetting = await context.AdminSettings.FirstOrDefaultAsync(
                i => i.Key.Equals(AdminSetting.IsBlackoutPeriod)
            );
            bool isBlackoutPeriod = Convert.ToBoolean(blackoutPeriodSetting.Value);

            // If the blackout period is still on, this is a no-op.
            if (isBlackoutPeriod)
            {
                await logger.LogSuccess(
                    TaskEnum.BlackoutPeriodUpdate,
                    "Blackout period is still set. Not updating records."
                );
                return null;
            }

            // Otherwise, continue and update the blackout period.
            var candidateEmployees = context.Employees
                .Include(e => e.TimelineEntries)
                .Where(
                    e => (e.InviteDate.Equals(SurveyDateBuilder.BLACKOUT_DATE))
                )
                .ToList();

            var newDates = await SurveyDateBuilder.GetDatesBasedOnAdminSettings(context);

            foreach (Employee e in candidateEmployees)
            {
                try
                {
                    var employee = await UpdateBlackoutPeriod(e, newDates);
                    updatedEmployeeList.Add(employee);
                }
                catch (Exception exception)
                {
                    exceptionList.Add(
                        $"Exception updating blackout period of employee {e.FullName} " +
                        $"(ID: {e.GovernmentEmployeeId}): {exception.GetType()}: {exception.Message} "
                    );
                }
            }

            return new EmployeeTaskResult(
                TaskEnum.BlackoutPeriodUpdate,
                candidateEmployees.Count,
                updatedEmployeeList,
                exceptionList
            );
        }
    }
}