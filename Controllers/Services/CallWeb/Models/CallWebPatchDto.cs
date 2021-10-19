using NewJobSurveyAdmin.Models;

namespace NewJobSurveyAdmin.Services.CallWeb
{
    // The data transfer object to use when sending a PATCH request.
    public class CallWebPatchDto
    {
        public string Telkey { get; set; }
        public string PreferredEmail { get; set; }
        public string PreferredFirstName { get; set; }
        public string CurrentStatus { get; set; }
        public string SurveyWindowFlag { get; set; }
        public string InviteDate { get; set; }
        public string Reminder1Date { get; set; }
        public string Reminder2Date { get; set; }
        public string DeadlineDate { get; set; }

        public static CallWebPatchDto FromEmployee(Employee employee)
        {
            return new CallWebPatchDto()
            {
                Telkey = employee.Telkey,
                PreferredEmail = employee.PreferredEmail,
                PreferredFirstName = employee.PreferredFirstName,
                CurrentStatus = employee.CurrentEmployeeStatusCode,
                InviteDate = employee.InviteDate.ToString("yyyy-MM-dd"),
                Reminder1Date = employee.Reminder1Date.ToString("yyyy-MM-dd"),
                Reminder2Date = employee.Reminder2Date.ToString("yyyy-MM-dd"),
                DeadlineDate = employee.DeadlineDate.ToString("yyyy-MM-dd"),
                SurveyWindowFlag = employee.SurveyWindowFlag(),
            };
        }
    }
}