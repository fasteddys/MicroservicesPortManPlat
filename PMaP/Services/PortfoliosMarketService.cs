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
    public interface IPortfoliosMarketService
    {
        Task<PortfoliosMarketResponse> GetAll(string Holder, string Project, string Year, string investor, string Tipology, string DebtType, string Value, bool isTableFilter = false);
    }

    public class PortfoliosMarketService : IPortfoliosMarketService
    {
        private readonly AppSettings _appSettings;

        public PortfoliosMarketService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<PortfoliosMarketResponse> GetAll(string Holder, string Project, string Year, string investor, string Tipology, string DebtType, string Value, bool isTableFilter = false)
        {
            string queryStrings = "?isTableFilter=" + isTableFilter;

            if (!string.IsNullOrEmpty(Holder))
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "Holder=" + Holder;
            if (!string.IsNullOrEmpty(Project))
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "Project=" + Project;
            if (!string.IsNullOrEmpty(investor))
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "investor=" + investor;
            if (!string.IsNullOrEmpty(Tipology))
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "Tipology=" + Tipology;
            if (!string.IsNullOrEmpty(DebtType))
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "DebtType=" + DebtType;
            if (!string.IsNullOrEmpty(Value))
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "Value=" + Value;
            if (!string.IsNullOrEmpty(Year))
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "Year=" + Year;

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
