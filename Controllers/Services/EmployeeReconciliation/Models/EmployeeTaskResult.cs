using NewJobSurveyAdmin.Models;

using System.Collections.Generic;

namespace NewJobSurveyAdmin.Services
{
    public class EmployeeTaskResult : GenericTaskResult<Employee>
    {
        public EmployeeTaskResult(TaskEnum task)
            : base(task) { }

        public EmployeeTaskResult(
            TaskEnum task,
            int candidateEmployeesCount,
            int ignoredEmployeesCount,
            List<Employee> goodEmployees,
            List<string> exceptions
        )
            : base(task, candidateEmployeesCount, ignoredEmployeesCount, goodEmployees, exceptions)
        { }
    }
}
