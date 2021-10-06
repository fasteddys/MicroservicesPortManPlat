using Microsoft.Extensions.Options;
using PMaP.Helpers;
using PMaP.Models.PortfoliosMarkets;
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
        private IHttpService _httpService;

        public PortfoliosMarketService(IOptions<AppSettings> appSettings, IHttpService httpService)
        {
            _appSettings = appSettings.Value;
            _httpService = httpService;
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

            return await _httpService.Get<PortfoliosMarketResponse>(_appSettings.PortfoliosMarketUrl + "/portfoliosMarket" + queryStrings) ?? new PortfoliosMarketResponse();
        }
    }
}
