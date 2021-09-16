using Contracts.Models;
using Contracts.Models.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Agreement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contracts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContractsController : ControllerBase
    {
        private pmpContext _pmpContext;

        public ContractsController(pmpContext pmpContext)
        {
            _pmpContext = pmpContext;
        }

        [HttpGet]
        public Response Get()
        {
            Response response = new Response();

            try
            {
                var contracts = _pmpContext.Contracts.ToList();
                response.ResponseCode = 200;
                response.Message = "Success";
                response.Documents = contracts;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet("portfolio/{id}")]
        public Response ContractByPortfolio(int id)
        {
            Response response = new Response();

            try
            {
                var portfolios = _pmpContext.Portfolios.Where(x => x.Id == id)
                    .Include(x => x.PortfolioContracts)
                    .ThenInclude(x => x.Contract)
                    .Include(x => x.PortfolioParticipants)
                    .ThenInclude(x => x.Participant)
                    .Include(x => x.PortfolioProcedures)
                    .ThenInclude(x => x.Procedure)
                    .Include(x => x.PortfolioInvestors)
                    .ThenInclude(x => x.Investor)
                    .ToList();

                List<Contract> contracts = new List<Contract>();
                List<Investor> investors = new List<Investor>();
                List<Participant> participants = new List<Participant>();
                List<Procedure> procedures = new List<Procedure>();
                foreach (var portfolio in portfolios)
                {
                    foreach (var portfolioContract in portfolio.PortfolioContracts.ToList())
                    {
                        portfolioContract.Portfolio = null;
                        if (portfolioContract.Contract != null)
                        {
                            portfolioContract.Contract.PortfolioNavigation = new Portfolio();
                            portfolioContract.Contract.PortfolioContracts = new List<PortfolioContract>();
                            portfolioContract.Contract.Investors = new List<Investor>();
                            portfolioContract.Contract.Participants = new List<Participant>();
                        }
                    }

                    foreach (var portfolioParticipant in portfolio.PortfolioParticipants.ToList())
                    {
                        portfolioParticipant.Portfolio = null;
                        if (portfolioParticipant.Participant != null)
                        {
                            portfolioParticipant.Participant.PortfolioNavigation = new Portfolio();
                            portfolioParticipant.Participant.PortfolioParticipants = new List<PortfolioParticipant>();
                            if (portfolioParticipant.Participant.ContractNavigation != null)
                            {
                                portfolioParticipant.Participant.ContractNavigation.Collaterals = new List<Collateral>();
                                portfolioParticipant.Participant.ContractNavigation.Investors = new List<Investor>();
                                portfolioParticipant.Participant.ContractNavigation.Participants = new List<Participant>();
                                portfolioParticipant.Participant.ContractNavigation.PortfolioContracts = new List<PortfolioContract>();
                                portfolioParticipant.Participant.ContractNavigation.PortfolioNavigation = new Portfolio();
                                portfolioParticipant.Participant.ContractNavigation.Prices = new List<Price>();
                                portfolioParticipant.Participant.ContractNavigation.Procedures = new List<Procedure>();
                            }
                        }
                    }

                    foreach (var portfolioInvestor in portfolio.PortfolioInvestors.ToList())
                    {
                        portfolioInvestor.Portfolio = new Portfolio();
                        if (portfolioInvestor.Investor != null)
                        {
                            portfolioInvestor.Investor.Portfolio = new Portfolio();
                            portfolioInvestor.Investor.PortfolioInvestors = new List<PortfolioInvestor>();
                            portfolioInvestor.Investor.Contract = new Contract();
                        }
                    }

                    foreach (var portfolioProcedure in portfolio.PortfolioProcedures.ToList())
                    {
                        portfolioProcedure.Portfolio = new Portfolio();
                        if (portfolioProcedure.Procedure != null)
                        {
                            portfolioProcedure.Procedure.PortfolioNavigation = new Portfolio();
                            portfolioProcedure.Procedure.PortfolioProcedures = new List<PortfolioProcedure>();
                            if (portfolioProcedure.Procedure.ContractNavigation != null)
                            {
                                portfolioProcedure.Procedure.ContractNavigation.Collaterals = new List<Collateral>();
                                portfolioProcedure.Procedure.ContractNavigation.Investors = new List<Investor>();
                                portfolioProcedure.Procedure.ContractNavigation.Participants = new List<Participant>();
                                portfolioProcedure.Procedure.ContractNavigation.PortfolioContracts = new List<PortfolioContract>();
                                portfolioProcedure.Procedure.ContractNavigation.PortfolioNavigation = new Portfolio();
                                portfolioProcedure.Procedure.ContractNavigation.Prices = new List<Price>();
                                portfolioProcedure.Procedure.ContractNavigation.Procedures = new List<Procedure>();
                            }
                        }
                    }

                    contracts = portfolio.PortfolioContracts.Select(x => x.Contract).ToList();
                    participants = portfolio.PortfolioParticipants.Select(x => x.Participant).ToList();
                    investors = portfolio.PortfolioInvestors.Select(x => x.Investor).ToList();
                    procedures = portfolio.PortfolioProcedures.Select(x => x.Procedure).ToList();
                }

                response.ResponseCode = 200;
                response.Message = "Success";
                response.Documents = contracts;
                response.Investors = investors;
                response.Participants = participants;
                response.Procedures = procedures;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet("portfolio/{id}/assessment")]
        public Response ContractsAssessment(int id)
        {
            Response response = new Response();

            try
            {
                var portfolioContracts = _pmpContext.PortfolioContracts.Where(x => x.PortfolioId == id).Include(x => x.Contract).ToList();

                var contracts = portfolioContracts.Select(x => x.Contract).ToList();
                //contracts = _pmpContext.Contracts.Where(x => x.PortfolioId == id).ToList();
                response.ResponseCode = 200;
                response.Message = "Success";
                response.Summary = new Summary
                {
                    Contracts = contracts.Count(),
                    Debtors = contracts.Sum(x => x.NumParticipants ?? 0),
                    Guarantors = contracts.Sum(x => x.NumGuarantors ?? 0),
                    SecuredOB = 0,
                    SecuredPrice = 0,
                    TotalOB = contracts.Sum(x => x.TotalAmountOb ?? 0),
                    UnsecuredOB = 0,
                    UnsecuredPrice = 0
                };
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet("{id}/portfolio/{portfolioId}")]
        public Response ContractByIdPortfolioId(int id, int portfolioId)
        {
            Response response = new Response();

            try
            {
                var portfolios = _pmpContext.Portfolios.Where(x => x.Id == portfolioId)
                    .Include(x => x.PortfolioContracts.Where(x => x.ContractId == id))
                    .ThenInclude(x => x.Contract)
                    .Include(x => x.PortfolioParticipants.Where(x => x.Participant.ContractId == id))
                    .ThenInclude(x => x.Participant)
                    .Include(x => x.PortfolioInvestors.Where(x => x.Investor.ContractId == id))
                    .ThenInclude(x => x.Investor)
                    .Include(x => x.PortfolioProcedures)
                    .ThenInclude(x => x.Procedure)
                    .ToList();

                List<Contract> contracts = new List<Contract>();
                List<Investor> investors = new List<Investor>();
                List<Participant> participants = new List<Participant>();
                List<Procedure> procedures = new List<Procedure>();
                foreach (var portfolio in portfolios)
                {
                    foreach (var portfolioContract in portfolio.PortfolioContracts.ToList())
                    {
                        portfolioContract.Portfolio = null;
                        if (portfolioContract.Contract != null)
                        {
                            portfolioContract.Contract.PortfolioNavigation = null;
                            portfolioContract.Contract.PortfolioContracts = null;
                            portfolioContract.Contract.Investors = null;
                            portfolioContract.Contract.Participants = null;
                        }
                    }

                    foreach (var portfolioParticipant in portfolio.PortfolioParticipants.ToList())
                    {
                        portfolioParticipant.Portfolio = null;
                        if (portfolioParticipant.Participant != null)
                        {
                            portfolioParticipant.Participant.PortfolioNavigation = null;
                            portfolioParticipant.Participant.PortfolioParticipants = null;
                            if (portfolioParticipant.Participant.ContractNavigation != null)
                            {
                                portfolioParticipant.Participant.ContractNavigation.Collaterals = null;
                                portfolioParticipant.Participant.ContractNavigation.Investors = null;
                                portfolioParticipant.Participant.ContractNavigation.Participants = null;
                                portfolioParticipant.Participant.ContractNavigation.PortfolioContracts = null;
                                portfolioParticipant.Participant.ContractNavigation.PortfolioNavigation = null;
                                portfolioParticipant.Participant.ContractNavigation.Prices = null;
                                portfolioParticipant.Participant.ContractNavigation.Procedures = null;
                            }
                        }
                    }

                    foreach (var portfolioInvestor in portfolio.PortfolioInvestors.ToList())
                    {
                        portfolioInvestor.Portfolio = null;
                        if (portfolioInvestor.Investor != null)
                        {
                            portfolioInvestor.Investor.Portfolio = null;
                            portfolioInvestor.Investor.PortfolioInvestors = null;
                            portfolioInvestor.Investor.Contract = null;
                        }
                    }

                    foreach (var portfolioProcedure in portfolio.PortfolioProcedures.ToList())
                    {
                        portfolioProcedure.Portfolio = null;
                        if (portfolioProcedure.Procedure != null)
                        {
                            portfolioProcedure.Procedure.PortfolioNavigation = null;
                            portfolioProcedure.Procedure.PortfolioProcedures = null;
                            if (portfolioProcedure.Procedure.ContractNavigation != null)
                            {
                                portfolioProcedure.Procedure.ContractNavigation.Collaterals = null;
                                portfolioProcedure.Procedure.ContractNavigation.Investors = null;
                                portfolioProcedure.Procedure.ContractNavigation.Participants = null;
                                portfolioProcedure.Procedure.ContractNavigation.PortfolioContracts = null;
                                portfolioProcedure.Procedure.ContractNavigation.PortfolioNavigation = null;
                                portfolioProcedure.Procedure.ContractNavigation.Prices = null;
                                portfolioProcedure.Procedure.ContractNavigation.Procedures = null;
                            }
                        }
                    }

                    contracts = portfolio.PortfolioContracts.Select(x => x.Contract).ToList();
                    participants = portfolio.PortfolioParticipants.Select(x => x.Participant).ToList();
                    investors = portfolio.PortfolioInvestors.Select(x => x.Investor).ToList();
                    procedures = portfolio.PortfolioProcedures.Select(x => x.Procedure).ToList();
                }

                response.ResponseCode = 200;
                response.Message = "Success";
                response.Documents = contracts;
                response.Investors = investors;
                response.Participants = participants;
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
