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
            // If the task result is null,  this is a no-op.
            if (taskResult == null)
            {
                return Task.FromResult<TaskLogEntry>(null);
            }

            return Log(taskResult.Task, taskResult.TaskOutcome, taskResult.Message);
        }
    }
}