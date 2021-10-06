using Microsoft.Extensions.Options;
using PMaP.Helpers;
using PMaP.Models.Portfolios;
using System;
using System.Threading.Tasks;

namespace PMaP.Services
{
    public interface IPortfolioService
    {
        Task<PortfolioResponse> GetAll(string portfolio, string subportfolio, DateTime? creation_date, DateTime? cut_off_date, DateTime? signing_date, DateTime? closing_date,
            string holderEntity, string investor, decimal? closingOB, string typology, int? year, int? contracts, string status, bool isTableFilter = false);
    }

    public class PortfolioService : IPortfolioService
    {
        private readonly AppSettings _appSettings;
        private IHttpService _httpService;

        public PortfolioService(IOptions<AppSettings> appSettings, IHttpService httpService)
        {
            _appSettings = appSettings.Value;
            _httpService = httpService;
        }

        public async Task<PortfolioResponse> GetAll(string portfolio, string subportfolio, DateTime? creation_date, DateTime? cut_off_date, DateTime? signing_date, DateTime? closing_date,
            string holderEntity, string investor, decimal? closingOB, string typology, int? year, int? contracts, string status, bool isTableFilter = false)
        {
            string queryStrings = "?isTableFilter=" + isTableFilter;

            if (!string.IsNullOrEmpty(portfolio))
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "portfolio=" + portfolio;
            if (!string.IsNullOrEmpty(holderEntity))
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "holderEntity=" + holderEntity;
            if (!string.IsNullOrEmpty(subportfolio))
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "subportfolio=" + subportfolio;
            if (!string.IsNullOrEmpty(investor))
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "investor=" + investor;
            if (!string.IsNullOrEmpty(typology))
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "typology=" + typology;
            if (closingOB != null && closingOB > 0)
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "closingOB=" + closingOB;
            if (contracts != null && contracts > 0)
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "contracts=" + contracts;
            if (year != null && year > 0)
                queryStrings += (!string.IsNullOrEmpty(queryStrings) ? "&" : "?") + "year=" + year;

            return await _httpService.Get<PortfolioResponse>(_appSettings.PortfolioUrl + "/portfolios" + queryStrings) ?? new PortfolioResponse();
        }
    }
}
