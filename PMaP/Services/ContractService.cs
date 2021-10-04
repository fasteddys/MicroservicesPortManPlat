using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PMaP.Helpers;
using PMaP.Models.Contracts;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PMaP.Services
{
    public class ContractService : IContractService
    {
        private readonly AppSettings _appSettings;

        public ContractService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<ContractResponse> GetAll()
        {
            ContractResponse contractResponse = new ContractResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ContractUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = await client.GetAsync("contracts");

                var result = responseTask;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    contractResponse = JsonConvert.DeserializeObject<ContractResponse>(readTask);
                }
            }

            return contractResponse;
        }

        public async Task<ContractResponse> ContractByPortfolio(int id)
        {
            ContractResponse contractResponse = new ContractResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ContractUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = await client.GetAsync("contracts/portfolio/" + id);

                var result = responseTask;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    contractResponse = JsonConvert.DeserializeObject<ContractResponse>(readTask);
                }
            }

            return contractResponse;
        }

        public async Task<ContractResponse> ContractsAssessment(int id)
        {
            ContractResponse contractResponse = new ContractResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ContractUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = await client.GetAsync("contracts/portfolio/" + id + "/assessment");

                var result = responseTask;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    contractResponse = JsonConvert.DeserializeObject<ContractResponse>(readTask);
                }
            }

            return contractResponse;
        }

        public async Task<ContractResponse> ContractByIdPortfolioId(int id, int portfolioId)
        {
            ContractResponse contractResponse = new ContractResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.ContractUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = await client.GetAsync("contracts/" + id + "/portfolio/" + portfolioId);

                var result = responseTask;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    contractResponse = JsonConvert.DeserializeObject<ContractResponse>(readTask);
                }
            }

            return contractResponse;
        }
    }
}
