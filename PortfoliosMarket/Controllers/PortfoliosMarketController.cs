using Microsoft.AspNetCore.Mvc;
using Portfolios.Models;
using PortfoliosMarket.Models.DBModels;
using System;
using System.Linq;

namespace PortfoliosMarket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PortfoliosMarketController : ControllerBase
    {
        private readonly pmpContext _pmpContext;

        public PortfoliosMarketController(pmpContext pmpContext)
        {
            _pmpContext = pmpContext;
        }

        [HttpGet]
        public Response Get(string Holder, string Project, string Year, string investor,  string Tipology,  string DebtType, string Value, bool isTableFilter = false)
        {
            Response response = new Response();

            try
            {
                var portfolios = _pmpContext.PortfolioMarkets.AsQueryable();
                if (isTableFilter)
                {
                    if (!string.IsNullOrEmpty(Holder))
                        portfolios = portfolios.Where(x => x.Holder.ToLower().Contains(Holder.ToLower()));
                    if (!string.IsNullOrEmpty(Project))
                        portfolios = portfolios.Where(x => x.Project.ToLower().Contains(Project.ToLower()));
                }
                else
                {
                    if (!string.IsNullOrEmpty(Holder))
                        portfolios = portfolios.Where(x => x.Holder.ToLower() == Holder.ToLower());
                    if (!string.IsNullOrEmpty(Project))
                        portfolios = portfolios.Where(x => x.Project.ToLower() == Project.ToLower());
                }

                if (!string.IsNullOrEmpty(investor))
                    portfolios = portfolios.Where(x => x.Investor.ToLower().Contains(investor.ToLower()));
                if (!string.IsNullOrEmpty(Year))
                    portfolios = portfolios.Where(x => x.Year.ToLower().Contains(Year.ToLower()));
                if (!string.IsNullOrEmpty(Tipology))
                    portfolios = portfolios.Where(x => x.Typology.ToLower().Contains(Tipology.ToLower()));
                if (!string.IsNullOrEmpty(DebtType))
                    portfolios = portfolios.Where(x => x.DebtType.ToLower().Contains(DebtType.ToLower()));
                if (!string.IsNullOrEmpty(Value))
                    portfolios = portfolios.Where(x => x.Value.ToLower().Contains(Value.ToLower()));
                
                var dataPortfolios = portfolios.ToList();

                dataPortfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio = null));
                dataPortfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.PortfolioNavigation = null));
                dataPortfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.PortfolioContracts = null));
                dataPortfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.Investors = null));
                dataPortfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.Participants = null));

                dataPortfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Portfolio = null));
                dataPortfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.PortfolioNavigation = null));
                dataPortfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.PortfolioParticipants = null));
                dataPortfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation = null));

                dataPortfolios.ForEach(x => x.PortfolioInvestors.ToList().ForEach(x => x.Portfolio = null));
                dataPortfolios.ForEach(x => x.PortfolioInvestors.ToList().ForEach(x => x.Investor.Portfolio = null));
                dataPortfolios.ForEach(x => x.PortfolioInvestors.ToList().ForEach(x => x.Investor.PortfolioInvestors = null));
                dataPortfolios.ForEach(x => x.PortfolioInvestors.ToList().ForEach(x => x.Investor.Contract = null));

                foreach (var item in dataPortfolios)
                {
                    item.ContractsNavigation = item.PortfolioContracts.Select(x => x.Contract).ToList();
                    item.Participants = item.PortfolioParticipants.Select(x => x.Participant).ToList();
                    item.Investors = item.PortfolioInvestors.Select(x => x.Investor).ToList();
                }

                dataPortfolios.ForEach(x => x.PortfolioContracts = null);
                dataPortfolios.ForEach(x => x.PortfolioInvestors = null);
                dataPortfolios.ForEach(x => x.PortfolioParticipants = null);

                response.ResponseCode = 200;
                response.Message = "Success";
                response.PortfolioMarkets = dataPortfolios;
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
