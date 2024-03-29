using System;
using System.Linq;
using System.Collections.Generic;

namespace NewJobSurveyAdmin.Services
{
    // Tracks successes and failures of operations on a generic T (in typical
    // use, this will be an Employee). The consumer is responsible for adding
    // ranges of successes / failures and their associated Exceptions.
    public class TaskResult<T>
    {
        private static string NEW_LINE = System.Environment.NewLine;

        public TaskResult()
        {
            this.Ignored = new List<T>();
            this.Succeeded = new List<T>();
            this.Failed = new List<T>();
            this.Exceptions = new List<Exception>();
        }

        public TaskResult(
            List<T> ignored,
            List<T> succeeded,
            List<T> failed,
            List<Exception> exceptions
        )
        {
            this.Ignored = ignored;
            this.Succeeded = succeeded;
            this.Failed = failed;
            this.Exceptions = exceptions;
        }

        // A user has been "ignored" if we examined them and realized that they
        // did not need an operation applied.
        public List<T> Ignored { get; set; }
        public List<T> Succeeded { get; set; }
        public List<T> Failed { get; set; }
        public List<Exception> Exceptions { get; set; }

        public int IgnoredCount
        {
            get { return Ignored.Count; }
        }

        public int SucceededCount
        {
            get { return Succeeded.Count; }
        }

        public int FailedCount
        {
            get { return Failed.Count; }
        }

        public int ExceptionCount
        {
            get { return Exceptions.Count; }
        }

        public List<string> ExceptionMessages
        {
            get { return Exceptions.Select(ex => ex.Message).ToList(); }
        }

        public int TotalRecordCount
        {
            get { return IgnoredCount + SucceededCount + FailedCount; }
        }

        public bool HasExceptions
        {
            get { return (ExceptionCount > 0); }
        }

        public void AddIgnored(T ignored)
        {
            this.Ignored.Add(ignored);
        }

        public void AddIgnored(IEnumerable<T> ignored)
        {
            this.Ignored.AddRange(ignored);
        }

        public void AddSucceeded(T succeeded)
        {
            this.Succeeded.Add(succeeded);
        }

        public void AddSucceeded(IEnumerable<T> succeeded)
        {
            this.Succeeded.AddRange(succeeded);
        }

        public void AddFailed(T failed)
        {
            this.Failed.Add(failed);
        }

        public void AddFailed(IEnumerable<T> failed)
        {
            this.Failed.AddRange(failed);
        }

        public void AddException(Exception exception)
        {
            this.Exceptions.Add(exception);
        }

        public void AddExceptions(IEnumerable<Exception> exceptions)
        {
            this.Exceptions.AddRange(exceptions);
        }

        public void AddFailedWithException(T failed, Exception exception)
        {
            this.Failed.Add(failed);
            this.Exceptions.Add(exception);
        }

        // Adds the failures and exceptions from another TaskResult, while
        // returning the successes. In practical use, this makes it possible
        // to perform a process returning a TaskResult on a list of (e.g.)
        // Employees, store the failures + exceptions, and then have a list of
        // Employees for whom the operation was successful to work with in the
        // next step of the operation.
        public List<T> AddIncremental(TaskResult<T> otherTaskResult)
        {
            this.CopyFailedIgnoredAndExceptionsFrom(otherTaskResult);
            return otherTaskResult.Succeeded;
        }

        // Adds all successes, failures and exceptions from another TaskResult.
        // Ideally, called with the results of a final step in a process,
        // before "this" TaskResult is returned.
        public void AddFinal(TaskResult<T> otherTaskResult)
        {
            this.CopyFrom(otherTaskResult);
        }

        public void CopyFailedIgnoredAndExceptionsFrom(TaskResult<T> otherTaskResult)
        {
            this.AddIgnored(otherTaskResult.Ignored);
            this.AddFailed(otherTaskResult.Failed);
            this.AddExceptions(otherTaskResult.Exceptions);
        }

        public void CopyFrom(TaskResult<T> otherTaskResult)
        {
            this.CopyFailedIgnoredAndExceptionsFrom(otherTaskResult);
            this.AddSucceeded(otherTaskResult.Succeeded);
        }
    }
}
