using Investors.Models;
using Investors.Models.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Investors.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvestorsController : ControllerBase
    {
        private pmpContext _pmpContext;

        public InvestorsController(pmpContext pmpContext)
        {
            _pmpContext = pmpContext;
        }

        [HttpGet]
        public Response Get()
        {
            Response response = new Response();
            try
            {
                response.ResponseCode = 200;
                response.Message = "Success";
                response.Investors = _pmpContext.Investors.ToList();
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet("portfolio/{id}")]
        public Response InvestorsByPortfolio(int id)
        {
            Response response = new Response();
            try
            {
                var contracts = _pmpContext.Contracts.Where(x => x.PortfolioId == id).Include(x => x.Investors).ToList();
                contracts.ForEach(x => x.Investors.ToList().ForEach(y => y.Contract = null));

                List<Investor> investors = new List<Investor>();
                foreach (var contract in contracts)
                {
                    investors.AddRange(contract.Investors);
                }

                response.ResponseCode = 200;
                response.Message = "Success";
                response.Investors = investors;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
