using Microsoft.AspNetCore.Mvc;
using PMaP.Services;
using System.Threading.Tasks;

namespace PMaP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfoliosMarketController : ControllerBase
    {
        private IPortfoliosMarketService _portfoliosMarketService;

        public PortfoliosMarketController(IPortfoliosMarketService portfoliosMarketService)
        {
            _portfoliosMarketService = portfoliosMarketService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(string queryStrings)
        {
            var portfoliosMarkets = await _portfoliosMarketService.GetAll(queryStrings);
            return Ok(portfoliosMarkets);
        }
    }
}
