using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PMaP.Helpers;
using PMaP.Models.DBModels;
using PMaP.Models.PortfolioEvaluations;
using PMaP.Models.Portfolios;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PMaP.Services
{
    public class PortfolioEvaluationService : IPortfolioEvaluationService
    {
        private readonly AppSettings _appSettings;

        public PortfolioEvaluationService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<PortfolioEvaluationResponse> GetSummary(PortfolioEvaluationRequest request)
        {
            PortfolioEvaluationResponse portfolioEvaluationResponse = new PortfolioEvaluationResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.PortfolioEvaluationUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                //HTTP POST
                var responseTask = await client.PostAsync("portfolioValuation/summary", content);

                var result = responseTask;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    portfolioEvaluationResponse = JsonConvert.DeserializeObject<PortfolioEvaluationResponse>(readTask);
                }
            }

            return portfolioEvaluationResponse;
        }

        public async Task<PortfolioEvaluationResponse> ContractList(PortfolioEvaluationRequest request)
        {
            PortfolioEvaluationResponse portfolioEvaluationResponse = new PortfolioEvaluationResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.PortfolioEvaluationUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                //HTTP POST
                var responseTask = await client.PostAsync("portfolioValuation/details/contracts", content);

                var result = responseTask;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    portfolioEvaluationResponse = JsonConvert.DeserializeObject<PortfolioEvaluationResponse>(readTask);
                }
            }

            return portfolioEvaluationResponse;
        }

        public async Task<PortfolioEvaluationResponse> AddPortfolio(AddPortfolioRequest request)
        {
            PortfolioEvaluationResponse portfolioEvaluationResponse = new PortfolioEvaluationResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.PortfolioEvaluationUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                //HTTP POST
                var responseTask = await client.PostAsync("portfolioValuation/portfolio", content);

                var result = responseTask;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    portfolioEvaluationResponse = JsonConvert.DeserializeObject<PortfolioEvaluationResponse>(readTask);
                }
            }

            return portfolioEvaluationResponse;
        }

        public async Task<PortfolioEvaluationResponse> UpdatePortfolio(UpdatePortfolioRequest request)
        {
            PortfolioEvaluationResponse portfolioEvaluationResponse = new PortfolioEvaluationResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.PortfolioEvaluationUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                //HTTP PUT
                var responseTask = await client.PutAsync("portfolioValuation/portfolio", content);

                var result = responseTask;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    portfolioEvaluationResponse = JsonConvert.DeserializeObject<PortfolioEvaluationResponse>(readTask);
                }
            }

            return portfolioEvaluationResponse;
        }

        public async Task<PortfolioEvaluationResponse> DiscardPortfolio(int portfolioId)
        {
            PortfolioEvaluationResponse portfolioEvaluationResponse = new PortfolioEvaluationResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.PortfolioEvaluationUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP DELETE
                var responseTask = await client.DeleteAsync("portfolioValuation/portfolio/" + portfolioId);

                var result = responseTask;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    portfolioEvaluationResponse = JsonConvert.DeserializeObject<PortfolioEvaluationResponse>(readTask);
                }
            }

            return portfolioEvaluationResponse;
        }

        public async Task<PortfolioEvaluationResponse> DeletePortfolioContracts(int portfolioId, List<Contract> contracts)
        {
            PortfolioEvaluationResponse portfolioEvaluationResponse = new PortfolioEvaluationResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.PortfolioEvaluationUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(JsonConvert.SerializeObject(contracts), Encoding.UTF8, "application/json");
                //HTTP PUT
                var responseTask = await client.PutAsync("portfolioValuation/portfolio/" + portfolioId + "/contracts", content);

                var result = responseTask;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    portfolioEvaluationResponse = JsonConvert.DeserializeObject<PortfolioEvaluationResponse>(readTask);
                }
            }

            return portfolioEvaluationResponse;
        }
    }
}
