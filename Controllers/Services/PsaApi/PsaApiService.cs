using Microsoft.Extensions.Options;
using NewJobSurveyAdmin.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewJobSurveyAdmin.Services.PsaApi
{
    public class PsaApiService
    {
        private PsaApi PsaApi;
        private LoggingService logger;

        public PsaApiService(
            IOptions<PsaApiServiceOptions> options,
            IHttpClientFactory clientFactory,
            LoggingService logger
        )
        {
            PsaApi = new PsaApi(
                options.Value.NjsaDataUrl,
                options.Value.ClientUsername,
                options.Value.ClientPassword,
                clientFactory
            );
            this.logger = logger;
        }

        public async Task<EmployeeTaskResult> GetCurrent()
        {
            var taskResult = await PsaApi.GetAllEmployees(logger);

            await logger.LogEmployeeTaskResult(taskResult);

            return taskResult;
        }
    }
}