using Contracts.Models;
using Contracts.Models.DBModels;
using Microsoft.AspNetCore.Mvc;
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

                portfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio = null));
                portfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.PortfolioNavigation = null));
                portfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.PortfolioContracts = null));
                portfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.Investors = null));
                portfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.Participants = null));

                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Portfolio = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.PortfolioNavigation = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.PortfolioParticipants = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Collaterals = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Investors = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Participants = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.PortfolioContracts = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.PortfolioNavigation = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Prices = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Procedures = null));

                portfolios.ForEach(x => x.PortfolioInvestors.ToList().ForEach(x => x.Portfolio = null));
                portfolios.ForEach(x => x.PortfolioInvestors.ToList().ForEach(x => x.Investor.Portfolio = null));
                portfolios.ForEach(x => x.PortfolioInvestors.ToList().ForEach(x => x.Investor.PortfolioInvestors = null));
                portfolios.ForEach(x => x.PortfolioInvestors.ToList().ForEach(x => x.Investor.Contract = null));

                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Portfolio = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.PortfolioNavigation = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.PortfolioProcedures = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.ContractNavigation.Collaterals = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.ContractNavigation.Investors = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.ContractNavigation.Participants = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.ContractNavigation.PortfolioContracts = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.ContractNavigation.PortfolioNavigation = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.ContractNavigation.Prices = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.ContractNavigation.Procedures = null));

                List<Contract> contracts = new List<Contract>();
                List<Investor> investors = new List<Investor>();
                List<Participant> participants = new List<Participant>();
                List<Procedure> procedures = new List<Procedure>();
                foreach (var portfolio in portfolios)
                {
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

                portfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio = null));
                portfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.PortfolioNavigation = null));
                portfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.PortfolioContracts = null));
                portfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.Investors = null));
                portfolios.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract.Participants = null));

                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Portfolio = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.PortfolioNavigation = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.PortfolioParticipants = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Collaterals = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Investors = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Participants = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.PortfolioContracts = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.PortfolioNavigation = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Prices = null));
                portfolios.ForEach(x => x.PortfolioParticipants.ToList().ForEach(x => x.Participant.ContractNavigation.Procedures = null));

                portfolios.ForEach(x => x.PortfolioInvestors.ToList().ForEach(x => x.Portfolio = null));
                portfolios.ForEach(x => x.PortfolioInvestors.ToList().ForEach(x => x.Investor.Portfolio = null));
                portfolios.ForEach(x => x.PortfolioInvestors.ToList().ForEach(x => x.Investor.PortfolioInvestors = null));
                portfolios.ForEach(x => x.PortfolioInvestors.ToList().ForEach(x => x.Investor.Contract = null));

                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Portfolio = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.PortfolioNavigation = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.PortfolioProcedures = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.ContractNavigation.Collaterals = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.ContractNavigation.Investors = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.ContractNavigation.Participants = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.ContractNavigation.PortfolioContracts = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.ContractNavigation.PortfolioNavigation = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.ContractNavigation.Prices = null));
                portfolios.ForEach(x => x.PortfolioProcedures.ToList().ForEach(x => x.Procedure.ContractNavigation.Procedures = null));

                List<Contract> contracts = new List<Contract>();
                List<Investor> investors = new List<Investor>();
                List<Participant> participants = new List<Participant>();
                List<Procedure> procedures = new List<Procedure>();
                foreach (var portfolio in portfolios)
                {
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
