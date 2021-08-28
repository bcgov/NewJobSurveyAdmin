using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NewJobSurveyAdmin.Services;

namespace NewJobSurveyAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthStatusController : ControllerBase
    {

        private readonly EmployeeInfoLookupService infoLookupService;


        public HealthStatusController(EmployeeInfoLookupService infoLookupService)
        {
            this.infoLookupService = infoLookupService;
        }

        // GetStatus: Returns "Healthy." if the API is healthy.
        // GET: api/HealthStatus/Status
        [HttpGet("Status")]
        public ActionResult<string> GetStatus()
        {
            string text = "{ \"msg\": \"Healthy.\" }";

            return Ok(text);
        }


        [HttpGet("LdapCheck")]
        public async Task<ActionResult<string>> LdapCheck(string employeeId)
        {
            string email = this.infoLookupService.EmailByEmployeeId(employeeId);

            string text = "{ \"msg\": \"" + email + "\" }";

            return Ok(text);
        }
    }
}
