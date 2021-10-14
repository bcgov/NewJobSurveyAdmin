using NewJobSurveyAdmin.Models;
using NewJobSurveyAdmin.Services.CallWeb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin.Services
{
    public class EmployeeReconciliationService
    {


        private CallWebService callWeb;
        private NewJobSurveyAdminContext context;
        private EmployeeInfoLookupService infoLookupService;
        private LoggingService logger;

        public EmployeeReconciliationService(
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

        // NB. Existence is determined by employee ID and Effective Date.
        private Employee UniqueEmployeeExists(Employee candidate)
        {
            var query = context.Employees
                .Include(e => e.CurrentEmployeeStatus)
                .Where(e =>
                    e.GovernmentEmployeeId == candidate.GovernmentEmployeeId
                    && e.EffectiveDate == candidate.EffectiveDate
                );


            if (query.Count() > 0)
            {
                return query.First();
            }
            else
            {
                return null;
            }
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

        public async Task<EmployeeTaskResult> InsertEmployeesAndLog(
            List<Employee> candidateEmployees
        )
        {
            // Reconcile the employees with the database.
            var insertResult = await InsertEmployees(candidateEmployees);

            var newLine = System.Environment.NewLine;

            var message =
                $"From a list of {insertResult.TotalRecordCount} candidate employees, " +
                $"successfully inserted {insertResult.GoodRecordCount} rows. ";

            if (!insertResult.HasExceptions)
            {
                // No exceptions. Log a success.
                await logger.LogSuccess(TaskEnum.ReconcileEmployees, message);
            }
            else
            {
                message +=
                    $"There were {insertResult.ExceptionCount} employees with errors: " +
                    $"{string.Join(newLine, insertResult.Exceptions)} ";
                await logger.LogWarning(TaskEnum.ReconcileEmployees, message);
            }

            return insertResult;
        }

        private async Task<EmployeeTaskResult> InsertEmployees(
            List<Employee> employees
        )
        {
            var reconciledEmployeeList = new List<Employee>();
            var exceptionList = new List<string>();

            var dates = await SurveyDateBuilder.GetDatesBasedOnAdminSettings(context);

            // Step 1. Insert and update employees.
            foreach (Employee e in employees)
            {
                try
                {
                    var employee = await InsertEmployee(e, dates);
                    reconciledEmployeeList.Add(employee);
                }
                catch (Exception exception)
                {
                    exceptionList.Add(
                        $"Exception with candidate employee {e.FullName} " +
                        $"(ID: {e.GovernmentEmployeeId}): {exception.GetType()}: {exception.Message} "
                    );
                }
            }

            return new EmployeeTaskResult(reconciledEmployeeList, exceptionList);
        }

        private async Task<Employee> InsertEmployee(Employee employee, SurveyDates dates)
        {
            // Get the existing employee, if it exists.
            var existingEmployee = UniqueEmployeeExists(employee);

            if (existingEmployee == null)
            {
                // Case A. The employee does not exist in the database.

                // Set the status code for a new employee.
                var newStatusCode = EmployeeStatusEnum.Active.Code;
                employee.CurrentEmployeeStatusCode = newStatusCode;

                // Set the first name, last name, and email from LDAP, if
                // available.
                employee.UpdateInfoFromLdap(infoLookupService);

                // Set other preferred and calculated fields. This only runs the
                // first time the employee is created.
                employee.InstantiateFields(dates);

                // Try to insert a row into CallWeb, and set the telkey.
                try
                {
                    employee.Telkey = await callWeb.CreateSurvey(employee);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException(
                        "Inserting a row into CallWeb failed.", e
                    );
                }

                // Insert the employee into the database, along with an
                // appropriate timeline entry. Note that Ids are auto-generated.
                employee.TimelineEntries = new List<EmployeeTimelineEntry>();
                employee.TimelineEntries.Add(new EmployeeTimelineEntry
                {
                    EmployeeActionCode = EmployeeActionEnum.CreateFromCSV.Code,
                    EmployeeStatusCode = newStatusCode,
                    Comment = "Created automatically by script."
                });

                context.Employees.Add(employee);

                await context.SaveChangesAsync();

                // End Case A. Return the employee.
                return employee;
            }
            else
            {
                // Case B. The unique user DOES exist in the database.
                // In this case, for NJSA, we actually do nothing, but we do
                // throw an exception.
                throw new DuplicateEmployeeException(
                    $"Employee with ID {employee.GovernmentEmployeeId} and " +
                    $"hire date {employee.EffectiveDate.ToString("yyyy-MM-dd")} " +
                    "already exists.");
            }
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

        public async Task<EmployeeTaskResult> UpdateBlackoutPeriodsAndLog()
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
                return new EmployeeTaskResult(updatedEmployeeList, exceptionList);
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

            var updateResult = new EmployeeTaskResult(updatedEmployeeList, exceptionList);

            var newLine = System.Environment.NewLine;

            var message =
                $"Tried to update {candidateEmployees.Count} employee " +
                $"blackout periods. {updateResult.GoodRecordCount} were successful. ";

            if (!updateResult.HasExceptions)
            {
                // No exceptions. Log a success.
                await logger.LogSuccess(TaskEnum.BlackoutPeriodUpdate, message);
            }
            else
            {
                message +=
                    $"There were {updateResult.ExceptionCount} employees with errors: " +
                    $"{string.Join(newLine, updateResult.Exceptions)} ";
                await logger.LogWarning(TaskEnum.BlackoutPeriodUpdate, message);
            }

            return updateResult;
        }

        public async Task<Employee> UpdateEmployeeStatus(
            Employee employee
        )
        {
            var callWebStatusCode = await callWeb.GetSurveyStatusCode(employee);

            // Check if the employee has completed the survey.
            if (callWebStatusCode.Equals(EmployeeStatusEnum.SurveyComplete.Code))
            {
                await SaveStatusAndAddTimelineEntry(employee,
                    EmployeeStatusEnum.SurveyComplete);
            }

            // An employee only has a set amount of time to complete a survey.
            // If that time has expired, then expire the user.
            if (
                employee.DeadlineDate.AddDays(1) <= DateTime.UtcNow &&
                employee.CurrentEmployeeStatusCode != EmployeeStatusEnum.Expired.Code
            )
            {
                await SaveStatusAndAddTimelineEntry(employee,
                    EmployeeStatusEnum.Expired);
            }

            return employee;
        }

        public async Task<EmployeeTaskResult> UpdateEmployeeStatusesAndLog()
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

            var updateResult = new EmployeeTaskResult(updatedEmployeeList, exceptionList);

            var newLine = System.Environment.NewLine;

            var message =
                $"Tried to update {candidateEmployees.Count} employee " +
                $"statuses. {updateResult.GoodRecordCount} were successful. ";

            if (!updateResult.HasExceptions)
            {
                // No exceptions. Log a success.
                await logger.LogSuccess(TaskEnum.ReconcileEmployees, message);
            }
            else
            {
                message +=
                    $"There were {updateResult.ExceptionCount} employees with errors: " +
                    $"{string.Join(newLine, updateResult.Exceptions)} ";
                await logger.LogWarning(TaskEnum.ReconcileEmployees, message);
            }

            return updateResult;
        }
    }
}