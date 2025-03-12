using NewJobSurveyAdmin.Models;
using System;

namespace NewJobSurveyAdmin.Services.CallWeb
{
    public class CallWebPostDto
    {
        public string EmployeeId { get; set; }
        public string PreferredEmail { get; set; }
        public string PreferredFirstName { get; set; }
        public string LastName { get; set; }
        public string EffectiveDate { get; set; }
        public string CurrentStatus { get; set; }
        public string HireReason { get; set; }
        public string InviteDate { get; set; }
        public string Reminder1Date { get; set; }
        public string Reminder2Date { get; set; }
        public string DeadlineDate { get; set; }
        public string SurveyWindowFlag { get; set; }
        public string TaU7Flag { get; set; }
        public string LatTransferFlag { get; set; }
        public string NewHireFlag { get; set; }

        public static CallWebPostDto FromEmployee(Employee employee)
        {
            return new CallWebPostDto()
            {
                EmployeeId = employee.GovernmentEmployeeId,
                PreferredEmail = employee.PreferredEmail,
                PreferredFirstName = employee.PreferredFirstName,
                LastName = employee.LastName,
                EffectiveDate = employee.EffectiveDate.ToString("yyyy-MM-dd"),
                CurrentStatus = employee.CurrentEmployeeStatusCode,
                HireReason = employee.StaffingReason,
                InviteDate = employee.InviteDate.ToString("yyyy-MM-dd"),
                Reminder1Date = employee.Reminder1Date.ToString("yyyy-MM-dd"),
                Reminder2Date = employee.Reminder2Date.ToString("yyyy-MM-dd"),
                DeadlineDate = employee.DeadlineDate.ToString("yyyy-MM-dd"),
                SurveyWindowFlag = employee.SurveyWindowFlag(),
                TaU7Flag = employee.TaU7Flag(),
                LatTransferFlag = employee.LatTransferFlag(),
                NewHireFlag = employee.NewHireFlag()
            };
        }
    }
}