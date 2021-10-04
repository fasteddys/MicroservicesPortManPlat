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
}
