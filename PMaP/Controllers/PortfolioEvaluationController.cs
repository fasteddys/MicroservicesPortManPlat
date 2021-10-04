using Microsoft.AspNetCore.Mvc;
using PMaP.Models.DBModels;
using PMaP.Models.PortfolioEvaluations;
using PMaP.Models.Portfolios;
using PMaP.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMaP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioEvaluationController : ControllerBase
    {
        private IPortfolioEvaluationService _portfolioEvaluationService;

        public PortfolioEvaluationController(IPortfolioEvaluationService portfolioEvaluationService)
        {
            _portfolioEvaluationService = portfolioEvaluationService;
        }

        [Authorize]
        [HttpPost("summary")]
        public async Task<IActionResult> GetSummary(PortfolioEvaluationRequest request)
        {
            var summary = await _portfolioEvaluationService.GetSummary(request);
            return Ok(summary);
        }

        [Authorize]
        [HttpPost("details/contracts")]
        public async Task<IActionResult> ContractList(PortfolioEvaluationRequest request)
        {
            var contractList = await _portfolioEvaluationService.ContractList(request);
            return Ok(contractList);
        }

        [Authorize]
        [HttpPost("portfolio")]
        public async Task<IActionResult> AddPortfolio(AddPortfolioRequest request)
        {
            var addPortfolio = await _portfolioEvaluationService.AddPortfolio(request);
            return Ok(addPortfolio);
        }

        [Authorize]
        [HttpPut("portfolio")]
        public async Task<IActionResult> UpdatePortfolio(UpdatePortfolioRequest request)
        {
            var updatePortfolio = await _portfolioEvaluationService.UpdatePortfolio(request);
            return Ok(updatePortfolio);
        }

        [Authorize]
        [HttpDelete("portfolio/{portfolioId}")]
        public async Task<IActionResult> DiscardPortfolio(int portfolioId)
        {
            var discardPortfolio = await _portfolioEvaluationService.DiscardPortfolio(portfolioId);
            return Ok(discardPortfolio);
        }

        [Authorize]
        [HttpPut("portfolio/{portfolioId}/contracts")]
        public async Task<IActionResult> DeletePortfolioContracts(int portfolioId, List<Contract> contracts)
        {
            var deletePortfolioContracts = await _portfolioEvaluationService.DeletePortfolioContracts(portfolioId, contracts);
            return Ok(deletePortfolioContracts);
        }
    }
}
