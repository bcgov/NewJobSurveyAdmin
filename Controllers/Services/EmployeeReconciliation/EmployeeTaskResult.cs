using NewJobSurveyAdmin.Models;
using System.Collections.Generic;

namespace NewJobSurveyAdmin.Services
{
    public class EmployeeTaskResult
    {
        public EmployeeTaskResult(
            TaskEnum task,
            int candidateEmployeesCount,
            List<Employee> goodEmployees,
            List<string> exceptions
        )
        {
            this.Task = task;
            this.CandidateEmployeesCount = candidateEmployeesCount;
            this.GoodEmployees = goodEmployees;
            this.Exceptions = exceptions;
        }

        public string TaskVerb
        {
            get { return this.Task.Verb; }
        }

        public string TaskObjectNoun
        {
            get { return this.Task.ObjectNoun; }
        }

        public TaskEnum Task { get; set; }

        public int CandidateEmployeesCount { get; set; }

        public List<Employee> GoodEmployees { get; set; }

        public List<string> Exceptions { get; set; }

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