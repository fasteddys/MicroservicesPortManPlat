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
        public async Task<IActionResult> GetAll(string Holder, string Project, string Year, string investor, string Tipology, string DebtType, string Value, bool isTableFilter = false)
        {
            var portfoliosMarkets = await _portfoliosMarketService.GetAll(Holder, Project, Year, investor, Tipology, DebtType, Value, isTableFilter);
            return Ok(portfoliosMarkets);
        }
    }
}
