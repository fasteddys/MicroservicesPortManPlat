using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PMaP.Helpers;
using PMaP.Models.PortfoliosMarkets;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PMaP.Services
{
    public class PortfoliosMarketService : IPortfoliosMarketService
    {
        private readonly AppSettings _appSettings;

        public PortfoliosMarketService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<PortfoliosMarketResponse> GetAll(string queryStrings)
        {
            PortfoliosMarketResponse portfoliosMarketResponse = new PortfoliosMarketResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.PortfoliosMarketUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = await client.GetAsync("portfoliosMarket" + queryStrings);

                var result = responseTask;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    portfoliosMarketResponse = JsonConvert.DeserializeObject<PortfoliosMarketResponse>(readTask);
                }
            }

            return portfoliosMarketResponse;
        }
    }
}
