using NewJobSurveyAdmin.Models;
using System.Collections.Generic;

namespace NewJobSurveyAdmin.Services
{
    public class EmployeeTaskResult
    {
        public List<Employee> GoodEmployees { get; set; }
        public List<string> Exceptions { get; set; }

        public EmployeeTaskResult(
            List<Employee> goodEmployees,
            List<string> exceptions
        )
        {
            this.GoodEmployees = goodEmployees;
            this.Exceptions = exceptions;
        }

        public int GoodRecordCount
        {
            get { return GoodEmployees.Count; }
        }

        public int ExceptionCount
        {
            get { return Exceptions.Count; }
        }

        public int TotalRecordCount
        {
            get { return GoodRecordCount + ExceptionCount; }
        }

        public bool HasExceptions
        {
            get { return (ExceptionCount > 0); }
        }
    }
}