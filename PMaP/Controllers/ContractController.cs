using Microsoft.AspNetCore.Mvc;
using PMaP.Services;
using System.Threading.Tasks;

namespace PMaP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private IContractService _contractService;

        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contracts = await _contractService.GetAll();
            return Ok(contracts);
        }

        [Authorize]
        [HttpGet("portfolio/{id}")]
        public async Task<IActionResult> ContractByPortfolio(int id)
        {
            var contract = await _contractService.ContractByPortfolio(id);
            return Ok(contract);
        }

        [Authorize]
        [HttpGet("portfolio/{id}/assessment")]
        public async Task<IActionResult> ContractsAssessment(int id)
        {
            var contract = await _contractService.ContractsAssessment(id);
            return Ok(contract);
        }

        [Authorize]
        [HttpGet("{id}/portfolio/{portfolioId}")]
        public async Task<IActionResult> ContractByIdPortfolioId(int id, int portfolioId)
        {
            var contract = await _contractService.ContractByIdPortfolioId(id, portfolioId);
            return Ok(contract);
        }
    }
}
