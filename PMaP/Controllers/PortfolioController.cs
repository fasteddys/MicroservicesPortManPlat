using Microsoft.AspNetCore.Mvc;
using PMaP.Services;
using System.Threading.Tasks;

namespace PMaP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(string queryStrings)
        {
            var portfolios = await _portfolioService.GetAll(queryStrings);
            return Ok(portfolios);
        }
    }
}
