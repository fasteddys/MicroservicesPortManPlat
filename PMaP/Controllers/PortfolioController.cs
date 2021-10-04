using Microsoft.AspNetCore.Mvc;
using PMaP.Services;
using System;
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
        public async Task<IActionResult> GetAll(string portfolio, string subportfolio, DateTime? creation_date, DateTime? cut_off_date, DateTime? signing_date, DateTime? closing_date,
            string holderEntity, string investor, decimal? closingOB, string typology, int? year, int? contracts, string status, bool isTableFilter = false)
        {
            var portfolios = await _portfolioService.GetAll(portfolio, subportfolio, creation_date, cut_off_date, signing_date, closing_date,
                holderEntity, investor, closingOB, typology, year, contracts, status, isTableFilter);
            return Ok(portfolios);
        }
    }
}
