using NewJobSurveyAdmin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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
        private HttpClient GetClientWithBasicAuth()
        {

            var client = GetClient();

            var unencodedUsernamePassword = $"{ClientUsername}:{ClientPassword}";
            var encodedUsernamePassword = Convert.ToBase64String(System.Text.ASCIIEncoding.UTF8.GetBytes(unencodedUsernamePassword));

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", encodedUsernamePassword);

            return client;
        }

        private async Task<List<Employee>> EmployeesFromResponse(
            HttpResponseMessage response
        )
        {
            var responseAsString = await response.Content.ReadAsStringAsync();

            var settings = new JsonSerializerSettings();
            settings.DateFormatString = "YYYY-MM-DD";
            settings.ContractResolver = new PsaApiContractResolver();

            var jsonObject = JsonConvert.DeserializeObject<PsaApiRequestDto>(
                responseAsString,
                settings
            );

            return jsonObject.Employees;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            var client = GetClientWithBasicAuth();
            var response = await client.GetAsync($"{NjsaDataUrl}");
            var employees = await EmployeesFromResponse(response);

            return employees;
        }
    }
}