using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Participants.Models;
using Participants.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Participants.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private pmpContext _pmpContext;

        public ParticipantsController(pmpContext pmpContext)
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
                response.Participants = _pmpContext.Participants.ToList();
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet("portfolio/{id}")]
        public Response ParticipantsByPortfolio(int id)
        {
            Response response = new Response();

            try
            {
                var contracts = _pmpContext.Contracts.Where(x => x.PortfolioId == id).Include(x => x.Participants).ToList();
                contracts.ForEach(x => x.Participants.ToList().ForEach(y => y.ContractNavigation = null));

                List<Participant> participants = new List<Participant>();
                foreach (var contract in contracts)
                {
                    participants.AddRange(contract.Participants);
                }

                response.ResponseCode = 200;
                response.Message = "Success";
                response.Participants = participants;
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
