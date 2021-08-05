using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthStatusController : ControllerBase
    {
        public HealthStatusController()
        {
        }

        // GetStatus: Returns "Healthy." if the API is healthy.
        // GET: api/HealthStatus/Status
        [HttpGet("Status")]
        public ActionResult<string> GetStatus()
        {
            string text = "{ \"msg\": \"Healthy.\" }";

            return Ok(text);
        }
    }
}
