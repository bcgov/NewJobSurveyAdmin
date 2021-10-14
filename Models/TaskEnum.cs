using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewJobSurveyAdmin.Models
{
    public class TaskEnum
    {
        private static readonly string CodeReconcileEmployees = "ReconcileEmployees";
        private static readonly string CodeLoadPsa = "LoadPsa";
        private static readonly string CodeReadCsv = "ReadCsv";
        private static readonly string CodeLoadFromJson = "LoadFromJson";
        private static readonly string CodeLoadFromCsv = "LoadFromCsv";
        private static readonly string CodeRefreshStatuses = "RefreshStatuses";
        private static readonly string CodeRetrieveSurveyStatus = "RetrieveSurveyStatus";
        private static readonly string CodeScheduledTask = "ScheduledTask";
        private static readonly string CodeBlackoutPeriodUpdate = "BlackoutPeriodUpdate";

        public static readonly TaskEnum ReconcileEmployees = new TaskEnum
        {
            Code = CodeReconcileEmployees,
            Description = "The task to reconcile candidate employees with the existing database."
        };

        public static readonly TaskEnum LoadPsa = new TaskEnum
        {
            Code = CodeLoadPsa,
            Description = "The task to load data from the PSA API."
        };

        public static readonly TaskEnum ReadCsv = new TaskEnum
        {
            Code = CodeReadCsv,
            Description = "The task to read data from a supplied CSV."
        };

        public static readonly TaskEnum LoadFromJson = new TaskEnum
        {
            Code = CodeLoadFromJson,
            Description = "The task to insert POSTed employee JSON data into the database."
        };

        public static readonly TaskEnum LoadFromCsv = new TaskEnum
        {
            Code = CodeLoadFromCsv,
            Description = "The task to insert POSTed employee CSV data into the database."
        };

        public static readonly TaskEnum RefreshStatuses = new TaskEnum
        {
            Code = CodeRefreshStatuses,
            Description = "A manually-triggered refresh of employee statuses."
        };

        public static readonly TaskEnum RetrieveSurveyStatus = new TaskEnum
        {
            Code = CodeRetrieveSurveyStatus,
            Description = "The task to retrieve the survey completion statuses from CallWeb."
        };

        public static readonly TaskEnum ScheduledTask = new TaskEnum
        {
            Code = CodeScheduledTask,
            Description = "The scheduled task that runs daily, pulling from PSA API and updating statuses as required."
        };

        public static readonly TaskEnum BlackoutPeriodUpdate = new TaskEnum
        {
            Code = CodeBlackoutPeriodUpdate,
            Description = "The task to identify whether the blackout period is finished."
        };

        public static readonly List<TaskEnum> AllValues = new List<TaskEnum>
        {
            ReconcileEmployees,
            LoadPsa,
            ReadCsv,
            LoadFromJson,
            LoadFromCsv,
            RefreshStatuses,
            RetrieveSurveyStatus,
            ScheduledTask,
            BlackoutPeriodUpdate
        };

        [Key] [Required] public string Code { get; set; }

        [Required] public string Description { get; set; }

        [JsonIgnore] public virtual List<TaskLogEntry> TaskLogEntries { get; set; }
    }
}