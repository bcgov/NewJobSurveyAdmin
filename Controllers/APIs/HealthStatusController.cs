using Microsoft.AspNetCore.Mvc;
using NewJobSurveyAdmin.Services.CallWeb;
using NewJobSurveyAdmin.Services.PsaApi;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthStatusController : ControllerBase
    {
        private readonly CallWebService callWebService;
        private readonly PsaApiService psaApiService;

        public HealthStatusController(CallWebService callWebService, PsaApiService psaApiService)
        {
            this.callWebService = callWebService;
            this.psaApiService = psaApiService;
        }

        // GetStatus: Returns "Healthy." if the API is healthy.
        // GET: api/HealthStatus/Status
        [HttpGet("Status")]
        public ActionResult<string> GetStatus()
        {
            string text = "{ \"msg\": \"Healthy.\" }";

            return Ok(text);
        }

        // GetCallWebCount: Returns the count of records in CallWeb, if that API
        // is working, or -1 if there has been an error.
        // GET: api/HealthStatus/CallWebCount
        [HttpGet("CallWebCount")]
        public async Task<ActionResult<string>> GetCallWebCount()
        {
            var apiServiceCallResult = await this.callWebService.ListAll();

            int length = apiServiceCallResult == null
                ? -1
                : apiServiceCallResult.Length;

            string text = "{ \"callWebRecordCount\": \"" + length + "\" }";

            return Ok(text);
        }

        [HttpGet("PingPsaApi")]
        public async Task<ActionResult<string>> PingPsaApi()
        {
            var apiServiceCallResult = await this.psaApiService.GetCurrent();

            var text = "Text";

            return Ok(text);
        }
    }
}