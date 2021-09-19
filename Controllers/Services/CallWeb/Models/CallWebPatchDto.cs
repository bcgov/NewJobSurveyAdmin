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

        public static CallWebPatchDto FromEmployee(Employee employee)
        {
            return new CallWebPatchDto()
            {
                Telkey = employee.Telkey,
                PreferredEmail = employee.PreferredEmail,
                PreferredFirstName = employee.PreferredFirstName,
                CurrentStatus = employee.CurrentEmployeeStatusCode,
            };
        }
    }
}