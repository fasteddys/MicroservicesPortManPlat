using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PMaP.Helpers;
using PMaP.Models.Portfolios;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PMaP.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly AppSettings _appSettings;

        public PortfolioService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<PortfolioResponse> GetAll(string queryStrings)
        {
            PortfolioResponse portfolioResponse = new PortfolioResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.PortfolioUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP GET
                var responseTask = await client.GetAsync("portfolios" + queryStrings);

                var result = responseTask;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    portfolioResponse = JsonConvert.DeserializeObject<PortfolioResponse>(readTask);
                }
            }

            return portfolioResponse;
        }
    }
}
