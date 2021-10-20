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

        public async Task<EmployeeTaskResult> InsertEmployees(
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

            return new EmployeeTaskResult(
                TaskEnum.ReconcileEmployees,
                employees.Count,
                reconciledEmployeeList,
                exceptionList
            );
        }

        private async Task<Employee> InsertEmployee(
            Employee employee,
            SurveyDates dates
        )
        {
            // Check to see if an employee already exists.
            var existingEmployee = UniqueEmployeeExists(employee);

            if (existingEmployee == null)
            {
                // Case A. The employee does not exist in the database.

                // Set the status code for a new employee.
                var newStatusCode = EmployeeStatusEnum.Active.Code;
                employee.CurrentEmployeeStatusCode = newStatusCode;

                // Set info from LDAP. If no info is found, an exception will
                // be thrown.
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
    }
}