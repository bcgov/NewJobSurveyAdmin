using NewJobSurveyAdmin.Models;
using System;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin.Services
{
    public class LoggingService
    {
        private static string NEW_LINE = System.Environment.NewLine;

        private readonly NewJobSurveyAdminContext context;

        public LoggingService(NewJobSurveyAdminContext context)
        {
            this.context = context;
        }

        public async Task<TaskLogEntry> Log(
            TaskEnum task, TaskOutcomeEnum taskOutcome, string comment
        )
        {
            var entry = new TaskLogEntry()
            {
                TaskCode = task.Code,
                TaskOutcomeCode = taskOutcome.Code,
                Comment = comment
            };

            context.TaskLogEntries.Add(entry);
            await context.SaveChangesAsync();

            return entry;
        }

        public Task<TaskLogEntry> LogSuccess(TaskEnum task, string comment)
        {
            return Log(task, TaskOutcomeEnum.Success, comment);
        }

        public Task<TaskLogEntry> LogWarning(TaskEnum task, string comment)
        {
            return Log(task, TaskOutcomeEnum.Warn, comment);
        }

        public Task<TaskLogEntry> LogFailure(TaskEnum task, string comment)
        {
            return Log(task, TaskOutcomeEnum.Fail, comment);
        }

        public Task<TaskLogEntry> LogEmployeeTaskResult(EmployeeTaskResult taskResult)
        {
            // If the task result is null, or if we tried to address a total
            // of zero candidates, this is a no-op.
            if (taskResult == null || taskResult.CandidateEmployeesCount == 0)
            {
                return Task.FromResult<TaskLogEntry>(null);
            }

            var message =
                $"Tried to {taskResult.TaskVerb} " +
                $"{taskResult.CandidateEmployeesCount} " +
                $"{taskResult.TaskObjectNoun}. " +
                $"{taskResult.GoodRecordCount} were successful. ";

            if (!taskResult.HasExceptions)
            {
                // No exceptions. Log a success.
                return Log(taskResult.Task, TaskOutcomeEnum.Success, message);
            }
            else
            {
                // There were exceptions. Log them appropriately.
                message +=
                    $"There were {taskResult.ExceptionCount} errors: " +
                    $"{string.Join(NEW_LINE, taskResult.Exceptions)} ";

                return Log(taskResult.Task, TaskOutcomeEnum.Warn, message);
            }
        }
    }
}