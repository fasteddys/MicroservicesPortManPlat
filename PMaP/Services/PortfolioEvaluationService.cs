using Microsoft.Extensions.Options;
using PMaP.Helpers;
using PMaP.Models.DBModels;
using PMaP.Models.PortfolioEvaluations;
using PMaP.Models.Portfolios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMaP.Services
{
    public interface IPortfolioEvaluationService
    {
        Task<PortfolioEvaluationResponse> GetSummary(PortfolioEvaluationRequest request);
        Task<PortfolioEvaluationResponse> ContractList(PortfolioEvaluationRequest request);
        Task<PortfolioEvaluationResponse> AddPortfolio(AddPortfolioRequest request);
        Task<PortfolioEvaluationResponse> UpdatePortfolio(UpdatePortfolioRequest request);
        Task<PortfolioEvaluationResponse> DiscardPortfolio(int portfolioId);
        Task<PortfolioEvaluationResponse> DeletePortfolioContracts(int portfolioId, List<Contract> contracts);
    }

    public class PortfolioEvaluationService : IPortfolioEvaluationService
    {
        private readonly AppSettings _appSettings;
        private IHttpService _httpService;

        public PortfolioEvaluationService(IOptions<AppSettings> appSettings, IHttpService httpService)
        {
            _appSettings = appSettings.Value;
            _httpService = httpService;
        }

        public async Task<PortfolioEvaluationResponse> GetSummary(PortfolioEvaluationRequest request)
        {
            return await _httpService.Post<PortfolioEvaluationResponse>(_appSettings.PortfolioEvaluationUrl + "/portfolioValuation/summary", request) ?? new PortfolioEvaluationResponse();
        }

        public async Task<PortfolioEvaluationResponse> ContractList(PortfolioEvaluationRequest request)
        {
            return await _httpService.Post<PortfolioEvaluationResponse>(_appSettings.PortfolioEvaluationUrl + "/portfolioValuation/details/contracts", request) ?? new PortfolioEvaluationResponse();
        }

        public async Task<PortfolioEvaluationResponse> AddPortfolio(AddPortfolioRequest request)
        {
            return await _httpService.Post<PortfolioEvaluationResponse>(_appSettings.PortfolioEvaluationUrl + "/portfolioValuation/portfolio", request) ?? new PortfolioEvaluationResponse();
        }

        public async Task<PortfolioEvaluationResponse> UpdatePortfolio(UpdatePortfolioRequest request)
        {
            return await _httpService.Put<PortfolioEvaluationResponse>(_appSettings.PortfolioEvaluationUrl + "/portfolioValuation/portfolio", request) ?? new PortfolioEvaluationResponse();
        }

        public async Task<PortfolioEvaluationResponse> DiscardPortfolio(int portfolioId)
        {
            return await _httpService.Delete<PortfolioEvaluationResponse>(_appSettings.PortfolioEvaluationUrl + "/portfolioValuation/portfolio/" + portfolioId) ?? new PortfolioEvaluationResponse();
        }

        public async Task<PortfolioEvaluationResponse> DeletePortfolioContracts(int portfolioId, List<Contract> contracts)
        {
            return await _httpService.Put<PortfolioEvaluationResponse>(_appSettings.PortfolioEvaluationUrl + "/portfolioValuation/portfolio/" + portfolioId + "/contracts", contracts) ?? new PortfolioEvaluationResponse();
        }
    }
}
