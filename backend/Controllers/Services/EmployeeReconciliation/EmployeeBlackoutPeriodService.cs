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
        private EmployeeUpdateService updateService;

        public EmployeeBlackoutPeriodService(
            NewJobSurveyAdminContext context,
            CallWebService callWeb,
            LoggingService logger,
            EmployeeUpdateService updateService
        )
        {
            this.context = context;
            this.callWeb = callWeb;
            this.logger = logger;
            this.updateService = updateService;
        }

        public async Task<TaskResult<Employee>> UpdateBlackoutPeriod(
            List<Employee> employees,
            SurveyDates dates
        )
        {
            var taskResult = new TaskResult<Employee>();

            foreach (var employee in employees)
            {
                // Update employee dates.
                employee.SetDates(dates);
            }

            var employeesWithUpdatedSurveys = taskResult.AddIncremental(
                await callWeb.UpdateSurveys(employees)
            );

            foreach (var employee in employeesWithUpdatedSurveys)
            {
                // Create a new timeline entry.
                employee.TimelineEntries.Add(
                    new EmployeeTimelineEntry
                    {
                        EmployeeActionCode = EmployeeActionEnum.UpdateByTask.Code,
                        EmployeeStatusCode = employee.CurrentEmployeeStatusCode,
                        Comment =
                            $"Blackout dates unset by script. New dates: "
                            + $"InviteDate → {dates.InviteDate.ToString("yyyy-MM-dd")}, "
                            + $"Reminder1Date → {dates.Reminder1Date.ToString("yyyy-MM-dd")}, "
                            + $"Reminder2Date → {dates.Reminder2Date.ToString("yyyy-MM-dd")}, "
                            + $"DeadlineDate → {dates.DeadlineDate.ToString("yyyy-MM-dd")}."
                    }
                );
                context.Entry(employee).State = EntityState.Modified;
            }

            taskResult.AddFinal(
                await updateService.SaveExistingEmployees(employeesWithUpdatedSurveys)
            );

            return taskResult;
        }

        public async Task<EmployeeTaskResult> UpdateBlackoutPeriods()
        {
            var employeeTaskResult = new EmployeeTaskResult(TaskEnum.BlackoutPeriodUpdate);

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
            var employees = context.Employees
                .Include(e => e.TimelineEntries)
                .Where(e => (e.InviteDate.Equals(SurveyDateBuilder.BLACKOUT_DATE)))
                .ToList();

            var newDates = await SurveyDateBuilder.GetDatesBasedOnAdminSettings(context);

            // Do this in a batch, working with 100 employees at a time.
            var BATCH_SIZE = 100;
            var NUM_BATCHES = (int)Math.Ceiling((double)employees.Count() / BATCH_SIZE);
            for (var i = 0; i < NUM_BATCHES; i++)
            {
                var employeesInBatch = employees.Skip(i * BATCH_SIZE).Take(BATCH_SIZE).ToList();
                employeeTaskResult.AddFinalStep(
                    await UpdateBlackoutPeriod(employeesInBatch, newDates)
                );
            }

            return employeeTaskResult;
        }
    }
}
