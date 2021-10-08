using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NewJobSurveyAdmin.Services.PsaApi
{
    internal class PsaApi
    {
        private string NjsaDataUrl;
        private string ClientUsername;
        private string ClientPassword;

        private readonly IHttpClientFactory ClientFactory;

        public PsaApi(
            string njsaDataUrl,
            string clientUsername,
            string clientPassword,
            IHttpClientFactory clientFactory
        )
        {
            NjsaDataUrl = njsaDataUrl;
            ClientUsername = clientUsername;
            ClientPassword = clientPassword;
            ClientFactory = clientFactory;
        }


        private HttpClient GetClient()
        {
            // The HttpClientName is specified as a constant in Startup.cs.
            return ClientFactory.CreateClient(Startup.HttpClientName);
        }

        // Get a client that has had the Authorization header set to use the
        // access token.
        private async Task<HttpClient> GetClientWithBasicAuth()
        {

            var client = GetClient();

            var unencodedUsernamePassword = $"{ClientUsername}:{ClientPassword}";
            var encodedUsernamePassword = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(unencodedUsernamePassword));

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", encodedUsernamePassword);

            return client;
        }

        private StringContent ToJsonContent(object obj)
        {
            var serializedObj = JsonConvert.SerializeObject(obj);

            return new StringContent(
                serializedObj, Encoding.UTF8, "application/json"
            );
        }

        private async Task<object> PsaApiResultFromResponse(
            HttpResponseMessage response
        )
        {
            var responseAsString = await response.Content.ReadAsStringAsync();

            var jsonObject = JsonConvert.DeserializeObject(
                responseAsString
            );

            return jsonObject;
        }

        public async Task<object> GetAll()
        {
            var client = await GetClientWithBasicAuth();
            var response = await client.GetAsync($"{NjsaDataUrl}");
            var json = await PsaApiResultFromResponse(response);

            return json;
        }
    }
}