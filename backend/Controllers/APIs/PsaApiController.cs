using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewJobSurveyAdmin.Models;
using NewJobSurveyAdmin.Services.CallWeb;
using NewJobSurveyAdmin.Services.PsaApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin.Controllers
{
    [Authorize(Policy = "UserRole")]
    [Route("api/[controller]")]
    [ApiController]
    public class PsaApiController : ControllerBase
    {
        private readonly PsaApiService psaApiService;

        public PsaApiController(PsaApiService psaApiService)
        {
            this.psaApiService = psaApiService;
        }

        [HttpGet("CurrentEmployees")]
        public async Task<ActionResult<IList<Employee>>> CurrentEmployees()
        {
            var employees = await this.psaApiService.GetCurrent();

            return Ok(employees);
        }
    }
}
