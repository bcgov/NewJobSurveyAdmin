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

        public PsaApiService(
            IOptions<PsaApiServiceOptions> options,
            IHttpClientFactory clientFactory
        )
        {
            PsaApi = new PsaApi(
                options.Value.NjsaDataUrl,
                options.Value.ClientUsername,
                options.Value.ClientPassword,
                clientFactory
            );
        }

        public async Task<List<Employee>> GetCurrent()
        {
            var response = await PsaApi.GetAllEmployees();

            return response;
        }
    }
}