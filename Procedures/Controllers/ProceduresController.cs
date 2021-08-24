using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto.Agreement;
using Procedures.Models;
using Procedures.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Procedures.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProceduresController : ControllerBase
    {
        private pmpContext _pmpContext;

        public ProceduresController(pmpContext pmpContext)
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
                response.Procedures = _pmpContext.Procedures.ToList();
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet("portfolio/{id}")]
        public Response ProceduresByPortfolio(int id)
        {
            Response response = new Response();

            try
            {
                var contracts = _pmpContext.Contracts.Where(x => x.PortfolioId == id).Include(x => x.Procedures).ToList();
                contracts.ForEach(x => x.Procedures.ToList().ForEach(y => y.ContractNavigation = null));

                List<Procedure> procedures = new List<Procedure>();
                foreach (var contract in contracts)
                {
                    procedures.AddRange(contract.Procedures);
                }

                response.ResponseCode = 200;
                response.Message = "Success";
                response.Procedures = procedures;
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
