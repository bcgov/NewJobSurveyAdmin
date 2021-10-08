using System;
using NewJobSurveyAdmin.Models;
using Microsoft.Extensions.Options;
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

        public async Task<object> GetCurrent()
        {
            var response = await PsaApi.GetAll();

            return response;
        }
    }
}