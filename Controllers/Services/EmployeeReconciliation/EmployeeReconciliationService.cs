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

        public EmployeeReconciliationService(
            NewJobSurveyAdminContext context,
            CallWebService callWeb,
            EmployeeInfoLookupService infoLookupService
        )
        {
            this.context = context;
            this.callWeb = callWeb;
            this.infoLookupService = infoLookupService;
        }

        // NB. Existence is determined by employee ID.
        // TODO: Should there be other factors?
        private Employee EmployeeExists(
            Employee candidate
        )
        {
            var query = context.Employees
                .Include(e => e.CurrentEmployeeStatus)
                .Where(e =>
                    e.GovernmentEmployeeId == candidate.GovernmentEmployeeId
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

        public async Task<Tuple<List<Employee>, List<string>>> ReconcileEmployees(
            List<Employee> employees
        )
        {
            var reconciledEmployeeList = new List<Employee>();
            var exceptionList = new List<string>();

            // Step 1. Insert and update employees from the CSV.
            foreach (Employee e in employees)
            {
                try
                {
                    var employee = await ReconcileWithDatabase(e);
                    reconciledEmployeeList.Add(employee);
                }
                catch (Exception exception)
                {
                    exceptionList.Add(
                        $"Exception with candidate employee {e.FullName} " +
                        $"(ID: {e.GovernmentEmployeeId}): {exception} "
                    );
                }
            }

            return Tuple.Create(reconciledEmployeeList, exceptionList);
        }

        /*** Reconcile a single employee. NB! By default, this will NOT invoke
        other methods (such as status updating) that affect multiple other
        employees, unlike ReconcileEmployees which does so by default.
        */
        public async Task<Employee> ReconcileEmployee(Employee employee)
        {
            // Simply call the main ReconcileEmployees function, with this
            // single employee as the sole element of a list; then get the
            // employee from the resulting list.
            var result = await ReconcileEmployees(
                new List<Employee>() { employee }
            );
            var reconciledEmployee = result.Item1.ElementAt(0);

            return reconciledEmployee;
        }

        private async Task<Employee> ReconcileWithDatabase(Employee employee)
        {
            // Get the existing employee, if it exists.
            var existingEmployee = EmployeeExists(employee);

            if (existingEmployee == null)
            {
                // Case A. The employee does not exist in the database.

                // Set the status code for a new employee.
                var newStatusCode = EmployeeStatusEnum.Active.Code;
                employee.CurrentEmployeeStatusCode = newStatusCode;

                // Set the email.
                employee.UpdateEmail(infoLookupService);

                // Set other preferred fields. This only runs the first time
                // the employee is created.
                employee.InstantiateFields();

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
                // In this case, for NJSA, we actually do nothing.
                // TODO: At least log that a duplicate employee was found.
                return existingEmployee;
            }
        }


        public async Task<Employee> UpdateEmployeeStatus(
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

            return employee;
        }

        // public async Task UpdateNotExiting(List<Employee> reconciledEmployeeList)
        // {
        //     var activeDBEmployeesNotInCsv = context.Employees
        //         .Include(e => e.TimelineEntries)
        //         .Include(e => e.CurrentEmployeeStatus)
        //         .Where(e => e.CurrentEmployeeStatus.State != EmployeeStatusEnum.StateFinal) // Reproject this as the status might have changed
        //         .ToList()
        //         .Where(e => reconciledEmployeeList.All(e2 => e2.Id != e.Id)) // This finds all nonFinalEmployees whose Id is not in the reconciledEmployeeList
        //         .ToList();

        //     foreach (Employee e in activeDBEmployeesNotInCsv)
        //     {
        //         var employee = await SaveStatusAndAddTimelineEntry(
        //             e, EmployeeStatusEnum.NotExiting
        //         );
        //     }
        // }

        public async Task UpdateEmployeeStatuses()
        {
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
                var employee = await UpdateEmployeeStatus(e);
            }
        }
    }
}
