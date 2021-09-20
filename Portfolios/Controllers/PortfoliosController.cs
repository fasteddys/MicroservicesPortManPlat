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

                foreach (var item in dataPortfolios)
                {
                    foreach (var portfolioContracts in item.PortfolioContracts.ToList())
                    {
                        portfolioContracts.Portfolio = new Portfolio();
                        if (portfolioContracts.Contract != null)
                        {
                            portfolioContracts.Contract.PortfolioNavigation = new Portfolio();
                            portfolioContracts.Contract.PortfolioContracts = new List<PortfolioContract>();
                            portfolioContracts.Contract.Investors = new List<Investor>();
                            portfolioContracts.Contract.Participants = new List<Participant>();
                        }
                    }

                    foreach (var portfolioParticipants in item.PortfolioParticipants.ToList())
                    {
                        portfolioParticipants.Portfolio = new Portfolio();
                        if (portfolioParticipants.Participant != null)
                        {
                            portfolioParticipants.Participant.PortfolioNavigation = new Portfolio();
                            portfolioParticipants.Participant.PortfolioParticipants = new List<PortfolioParticipant>();
                            if (portfolioParticipants.Participant.ContractNavigation != null)
                            {
                                portfolioParticipants.Participant.ContractNavigation.Collaterals = new List<Collateral>();
                                portfolioParticipants.Participant.ContractNavigation.Investors = new List<Investor>();
                                portfolioParticipants.Participant.ContractNavigation.Participants = new List<Participant>();
                                portfolioParticipants.Participant.ContractNavigation.PortfolioContracts = new List<PortfolioContract>();
                                portfolioParticipants.Participant.ContractNavigation.PortfolioNavigation = new Portfolio();
                                portfolioParticipants.Participant.ContractNavigation.Prices = new List<Price>();
                                portfolioParticipants.Participant.ContractNavigation.Procedures = new List<Procedure>();
                            }
                        }
                    }

                    foreach (var portfolioInvestors in item.PortfolioInvestors.ToList())
                    {
                        portfolioInvestors.Portfolio = new Portfolio();
                        if (portfolioInvestors.Investor != null)
                        {
                            portfolioInvestors.Investor.Portfolio = new Portfolio();
                            portfolioInvestors.Investor.PortfolioInvestors = new List<PortfolioInvestor>();
                            portfolioInvestors.Investor.Contract = new Contract();
                        }
                    }

                    item.ContractsNavigation = item.PortfolioContracts.Select(x => x.Contract).ToList();
                    item.Participants = item.PortfolioParticipants.Select(x => x.Participant).ToList();
                    item.Investors = item.PortfolioInvestors.Select(x => x.Investor).ToList();
                }

                dataPortfolios.ForEach(x => x.PortfolioContracts = new List<PortfolioContract>());
                dataPortfolios.ForEach(x => x.PortfolioInvestors = new List<PortfolioInvestor>());
                dataPortfolios.ForEach(x => x.PortfolioParticipants = new List<PortfolioParticipant>());

                response.ResponseCode = 200;
                response.Message = "Success";
                response.Portfolios = dataPortfolios;
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
