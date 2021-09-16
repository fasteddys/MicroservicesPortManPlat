using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Portfolios.Models;
using Portfolios.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Portfolios.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PortfoliosController : ControllerBase
    {
        private readonly pmpContext _pmpContext;

        public PortfoliosController(pmpContext pmpContext)
        {
            _pmpContext = pmpContext;
        }

        [HttpGet]
        public Response Get(string portfolio, string subportfolio, DateTime? creation_date, DateTime? cut_off_date, DateTime? signing_date, DateTime? closing_date,
            string holderEntity, string investor, decimal? closingOB, string typology, int? year, int? contracts, string status, bool isTableFilter = false)
        {
            Response response = new Response();

            try
            {
                var portfolios = _pmpContext.Portfolios.Include(x => x.PortfolioContracts)
                    .ThenInclude(x => x.Contract)
                    .Include(x => x.PortfolioParticipants)
                    .ThenInclude(x => x.Participant)
                    .Include(x => x.PortfolioInvestors)
                    .ThenInclude(x => x.Investor)
                    .AsQueryable();
                if (isTableFilter)
                {
                    if (!string.IsNullOrEmpty(portfolio))
                        portfolios = portfolios.Where(x => x.Portfolio1.ToLower().Contains(portfolio.ToLower()));
                    if (!string.IsNullOrEmpty(subportfolio))
                        portfolios = portfolios.Where(x => x.Subportfolio.ToLower().Contains(subportfolio.ToLower()));
                }
                else
                {
                    if (!string.IsNullOrEmpty(portfolio))
                        portfolios = portfolios.Where(x => x.Portfolio1.ToLower() == portfolio.ToLower());
                    if (!string.IsNullOrEmpty(subportfolio))
                        portfolios = portfolios.Where(x => x.Subportfolio.ToLower() == subportfolio.ToLower());
                }
                if (creation_date != null)
                    portfolios = portfolios.Where(x => x.CreationDate == creation_date);
                if (cut_off_date != null)
                    portfolios = portfolios.Where(x => x.CutOffDate == cut_off_date);
                if (signing_date != null)
                    portfolios = portfolios.Where(x => x.SigningDate == signing_date);
                if (closing_date != null)
                    portfolios = portfolios.Where(x => x.ClosingDate == closing_date);
                if (!string.IsNullOrEmpty(holderEntity))
                    portfolios = portfolios.Where(x => x.HolderEntity.ToLower().Contains(holderEntity.ToLower()));
                if (!string.IsNullOrEmpty(investor))
                    portfolios = portfolios.Where(x => x.Investor.ToLower().Contains(investor.ToLower()));
                if (closingOB != null)
                    portfolios = portfolios.Where(x => x.ClosingOb.ToString().ToLower().Contains(closingOB.ToString().ToLower()));
                if (!string.IsNullOrEmpty(typology))
                    portfolios = portfolios.Where(x => x.Tipology.ToLower().Contains(typology.ToLower()));
                if (year != null)
                    portfolios = portfolios.Where(x => x.Year.ToString().ToLower().Contains(year.ToString().ToLower()));
                if (contracts != null)
                    portfolios = portfolios.Where(x => x.Contracts.ToString().ToLower().Contains(contracts.ToString().ToLower()));
                if (!string.IsNullOrEmpty(status))
                    portfolios = portfolios.Where(x => x.Status.ToLower().Contains(status.ToLower()));

                var dataPortfolios = portfolios.ToList();

                dataPortfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio = null));
                dataPortfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.PortfolioNavigation = null));
                dataPortfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.PortfolioContracts = null));
                dataPortfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.Investors = null));
                dataPortfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.Participants = null));

                dataPortfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Portfolio = null));
                dataPortfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.PortfolioNavigation = null));
                dataPortfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.PortfolioParticipants = null));
                dataPortfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Collaterals = null));
                dataPortfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Investors = null));
                dataPortfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Participants = null));
                dataPortfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.PortfolioContracts = null));
                dataPortfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.PortfolioNavigation = null));
                dataPortfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Prices = null));
                dataPortfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Procedures = null));

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
                response.Documents = dataPortfolios;
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
