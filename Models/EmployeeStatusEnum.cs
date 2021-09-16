using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewJobSurveyAdmin.Models
{
    public class EmployeeStatusEnum
    {
        private static readonly string CodeActive = "Active";
        private static readonly string CodeSurveyComplete = "SurveyComplete";
        private static readonly string CodeOutOfScope = "OutOfScope";
        private static readonly string CodeDeclined = "Declined";
        private static readonly string CodeExpired = "Expired";

        public static readonly string StateActive = "Active";
        public static readonly string StateFinal = "Final";

        public static readonly EmployeeStatusEnum Active = new EmployeeStatusEnum
        {
            Code = CodeActive,
            State = StateActive,
            Description = "Employee is active."
        };
        public static readonly EmployeeStatusEnum SurveyComplete = new EmployeeStatusEnum
        {
            Code = CodeSurveyComplete,
            State = StateFinal,
            Description = "Survey has been finished."
        };
        public static readonly EmployeeStatusEnum OutOfScope = new EmployeeStatusEnum
        {
            Code = CodeOutOfScope,
            State = StateFinal,
            Description = "Other ineligibility reason."
        };
        public static readonly EmployeeStatusEnum Declined = new EmployeeStatusEnum
        {
            Code = CodeDeclined,
            State = StateActive,
            Description = "The employee has asked not to complete the survey."
        };

        public static readonly EmployeeStatusEnum Expired = new EmployeeStatusEnum
        {
            Code = CodeExpired,
            State = StateFinal,
            Description = "The employee's effective date has passed without completing the survey."
        };

        public static readonly List<EmployeeStatusEnum> AllValues = new List<EmployeeStatusEnum>
        {
            Active,
            SurveyComplete,
            OutOfScope,
            Declined,
            Expired
        };

        public static Boolean IsActiveStatus(string statusCode)
        {
            var status = AllValues.Find(s => s.Code == statusCode);
            return status.State == StateActive;
        }


        [Key]
        [Required]
        public string Code { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Description { get; set; }

        [JsonIgnore]
        public virtual List<Employee> Employees { get; set; }

        [JsonIgnore]
        public virtual List<EmployeeTimelineEntry> TimelineEntries { get; set; }
    }
}