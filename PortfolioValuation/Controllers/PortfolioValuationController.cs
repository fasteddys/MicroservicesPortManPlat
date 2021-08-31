using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.CRUD;
using PortfolioValuation.Models;
using PortfolioValuation.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PortfolioValuation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PortfolioValuationController : ControllerBase
    {
        private pmpContext _pmpContext;
        private static bool reflectExcludedContractIds = true;

        public PortfolioValuationController(pmpContext pmpContext)
        {
            _pmpContext = pmpContext;
        }

        [HttpPost("summary")]
        public Response GetSummary(PortfolioValuationRequest request)
        {
            Response response = new Response();

            try
            {
                reflectExcludedContractIds = false;
                var contracts = GetContracts(request);

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

        [HttpPost("details/contracts")]
        public Response ContractList(PortfolioValuationRequest request)
        {
            Response response = new Response();

            try
            {
                var contracts = GetContracts(request);
                
                foreach (var contract in contracts)
                {
                    foreach (var portfolioContract in contract.PortfolioContracts)
                    {
                        var portfolio = _pmpContext.Portfolios.Where(x => x.Id == portfolioContract.PortfolioId).FirstOrDefault();
                        if (portfolio != null)
                        {
                            portfolioContract.Portfolio = portfolio;
                        }
                    }
                }

                contracts.ForEach(x => x.Participants.ToList().ForEach(x => x.ContractNavigation = null));
                contracts.ForEach(x => x.Investors.ToList().ForEach(x => x.Contract = null));
                contracts.ForEach(x => x.Procedures.ToList().ForEach(x => x.ContractNavigation = null));
                contracts.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Contract = null));
                contracts.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio.Collaterals = null));
                contracts.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio.Contracts = null));
                contracts.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio.Homes = null));
                contracts.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio.Insolvencies = null));
                contracts.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio.Investors = null));
                contracts.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio.Participants = null));
                contracts.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio.PortfolioContracts = null));
                contracts.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio.PortfolioInvestors = null));
                contracts.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio.PortfolioParticipants = null));
                contracts.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio.Prices = null));
                contracts.ForEach(x => x.PortfolioContracts.ToList().ForEach(x => x.Portfolio.Procedures = null));

                List<Participant> participants = new List<Participant>();
                List<Investor> investors = new List<Investor>();
                List<Procedure> procedures = new List<Procedure>();
                foreach (var contract in contracts)
                {
                    participants.AddRange(contract.Participants);
                    investors.AddRange(contract.Investors);
                    procedures.AddRange(contract.Procedures);
                }

                response.ResponseCode = 200;
                response.Message = "Success";
                response.Contracts = contracts;
                response.Participants = participants;
                response.Investors = investors;
                response.Procedures = procedures;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
            }

            reflectExcludedContractIds = true;
            return response;
        }

        [HttpPost("portfolio")]
        public async Task<Response> AddPortfolio(AddPortfolioRequest request)
        {
            Response response = new Response();

            try
            {
                var portfolioContext = _pmpContext.Portfolios.Where(x => x.Portfolio1.ToLower() == request.ViewModel.Portfolio.ToLower()).ToList();
                if (!string.IsNullOrEmpty(request.ViewModel.Subportfolio) && portfolioContext.Count() > 0)
                {
                    portfolioContext = portfolioContext.Where(x => x.Subportfolio.ToLower() == request.ViewModel.Subportfolio.ToLower()).ToList();
                }
                if (portfolioContext.Count() > 0)
                {
                    response.ResponseCode = 400;
                    response.Message = "Portfolio name already exist.";
                    return response;
                }

                List<PortfolioContract> portfolioContracts = new List<PortfolioContract>();
                List<PortfolioInvestor> portfolioInvestors = new List<PortfolioInvestor>();
                List<PortfolioParticipant> portfolioParticipants = new List<PortfolioParticipant>();

                var contractIds = request.Contracts.Select(x => x.Id).ToList();
                var investorIds = request.Investors.Select(x => x.Id).ToList();
                var participantIds = request.Participants.Select(x => x.Id).ToList();

                foreach (var contractId in contractIds)
                {
                    portfolioContracts.Add(new PortfolioContract { ContractId = contractId });
                }

                foreach (var investorId in investorIds)
                {
                    portfolioInvestors.Add(new PortfolioInvestor { InvestorId = investorId });
                }

                foreach (var participantId in participantIds)
                {
                    portfolioParticipants.Add(new PortfolioParticipant { ParticipantId = participantId });
                }

                Portfolio portfolio = new Portfolio
                {
                    Portfolio1 = request.ViewModel.Portfolio,
                    Subportfolio = request.ViewModel.Subportfolio,
                    OperationType = "SALE",
                    ClosingDate = request.ViewModel.DateClosing,
                    CreationDate = request.ViewModel.DateAdded,
                    CutOffDate = request.ViewModel.DateCutOff,
                    SigningDate = request.ViewModel.DateSigning,
                    PortfolioContracts = portfolioContracts,
                    PortfolioInvestors = portfolioInvestors,
                    PortfolioParticipants = portfolioParticipants
                };
                _pmpContext.Add(portfolio);
                await _pmpContext.SaveChangesAsync();

                response.ResponseCode = 200;
                response.Message = "Successfully added the portfolio.";
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpPut("portfolio")]
        public async Task<Response> UpdatePortfolio(UpdatePortfolioRequest request)
        {
            Response response = new Response();

            try
            {
                var portfolio = request.ViewModel.PortfolioValuationAdd.Portfolio;

                List<Investor> investors = new List<Investor>();
                List<Participant> participants = new List<Participant>();
                foreach (var item in request.Contracts)
                {
                    investors.AddRange(item.Investors);
                    participants.AddRange(item.Participants);
                }

                var contractIds = request.Contracts.Select(x => x.Id).ToList();
                var investorIds = investors.Select(x => x.Id).ToList();
                var participantIds = participants.Select(x => x.Id).ToList();
                
                // Remove existing data
                var portfolioContractsDelete = _pmpContext.PortfolioContracts.Where(x => x.PortfolioId == portfolio.Id && contractIds.Contains(x.ContractId ?? 0)).ToList();
                _pmpContext.RemoveRange(portfolioContractsDelete);

                var portfolioInvestorsDelete = _pmpContext.PortfolioInvestors.Where(x => x.PortfolioId == portfolio.Id && investorIds.Contains(x.InvestorId ?? 0)).ToList();
                _pmpContext.RemoveRange(portfolioInvestorsDelete);

                var portfolioParticipantsDelete = _pmpContext.PortfolioParticipants.Where(x => x.PortfolioId == portfolio.Id && participantIds.Contains(x.ParticipantId ?? 0)).ToList();
                _pmpContext.RemoveRange(portfolioParticipantsDelete);

                await _pmpContext.SaveChangesAsync();

                // prepare entities
                List<PortfolioContract> portfolioContracts = new List<PortfolioContract>();
                List<PortfolioInvestor> portfolioInvestors = new List<PortfolioInvestor>();
                List<PortfolioParticipant> portfolioParticipants = new List<PortfolioParticipant>();

                foreach (var contractId in contractIds)
                {
                    portfolioContracts.Add(new PortfolioContract { ContractId = contractId, PortfolioId = portfolio.Id });
                }
                foreach (var investorId in investorIds)
                {
                    portfolioInvestors.Add(new PortfolioInvestor { InvestorId = investorId, PortfolioId = portfolio.Id });
                }
                foreach (var participantId in participantIds)
                {
                    portfolioParticipants.Add(new PortfolioParticipant { ParticipantId = participantId, PortfolioId = portfolio.Id });
                }

                portfolio.PortfolioContracts = portfolioContracts;
                portfolio.PortfolioInvestors = portfolioInvestors;
                portfolio.PortfolioParticipants = portfolioParticipants;

                _pmpContext.Update(portfolio);
                await _pmpContext.SaveChangesAsync();

                response.ResponseCode = 200;
                response.Message = "Successfully added the contract/s to portfolio.";
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpDelete("portfolio/{portfolioId}")]
        public async Task<Response> DiscardPortfolio(int portfolioId)
        {
            Response response = new Response();

            try
            {
                var portfolio = _pmpContext.Portfolios.Where(x => x.Id == portfolioId).FirstOrDefault();
                if (portfolio != null)
                {
                    portfolio.OperationType = "DISCARD";

                    _pmpContext.Update(portfolio);
                    await _pmpContext.SaveChangesAsync();
                }

                response.ResponseCode = 200;
                response.Message = "Successfully discarded the portfolio.";
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpPut("portfolio/{portfolioId}/contracts")]
        public async Task<Response> DeletePortfolioContracts(int portfolioId, List<Contract> contracts)
        {
            Response response = new Response();

            try
            {
                var portfolio = _pmpContext.Portfolios.Where(x => x.Id == portfolioId).FirstOrDefault();
                if (portfolio == null)
                {
                    response.ResponseCode = 400;
                    response.Message = "Portfolio not found.";
                    return response;
                }

                List<Investor> investors = new List<Investor>();
                List<Participant> participants = new List<Participant>();
                foreach (var item in contracts)
                {
                    investors.AddRange(item.Investors);
                    participants.AddRange(item.Participants);
                }

                var contractIds = contracts.Select(x => x.Id).ToList();
                var investorIds = investors.Select(x => x.Id).ToList();
                var participantIds = participants.Select(x => x.Id).ToList();

                // Remove data
                var portfolioContractsDelete = _pmpContext.PortfolioContracts.Where(x => x.PortfolioId == portfolio.Id && contractIds.Contains(x.ContractId ?? 0)).ToList();
                _pmpContext.RemoveRange(portfolioContractsDelete);

                var portfolioInvestorsDelete = _pmpContext.PortfolioInvestors.Where(x => x.PortfolioId == portfolio.Id && investorIds.Contains(x.InvestorId ?? 0)).ToList();
                _pmpContext.RemoveRange(portfolioInvestorsDelete);

                var portfolioParticipantsDelete = _pmpContext.PortfolioParticipants.Where(x => x.PortfolioId == portfolio.Id && participantIds.Contains(x.ParticipantId ?? 0)).ToList();
                _pmpContext.RemoveRange(portfolioParticipantsDelete);

                await _pmpContext.SaveChangesAsync();

                response.ResponseCode = 200;
                response.Message = "Successfully updated the portfolio.";
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }

        private List<Contract> GetContracts(PortfolioValuationRequest request)
        {
            var query = _pmpContext.Contracts.AsQueryable();
            if (request.From != null)
                query = query.Where(x => x.ProcessDate >= request.From);
            if (request.To != null)
                query = query.Where(x => x.ProcessDate <= request.To);
            if (!string.IsNullOrEmpty(request.DebtType))
                query = query.Where(x => x.DebtType == request.DebtType);
            if (!string.IsNullOrEmpty(request.Judicialized))
                query = query.Where(x => x.JudicialProcess == Convert.ToByte(request.Judicialized));
            if (!string.IsNullOrEmpty(request.Insolvency))
                query = query.Where(x => x.Insolvency == request.Insolvency);
            if (!string.IsNullOrEmpty(request.PerformingStatus))
                query = query.Where(x => x.PerformingStatus == request.PerformingStatus);
            if (!string.IsNullOrEmpty(request.DebtOB))
            {
                switch (request.DebtOB)
                {
                    case "1000":
                        query = query.Where(x => x.TotalAmountOb <= 1000);
                        break;
                    case "4999":
                        query = query.Where(x => x.TotalAmountOb > 1000 && x.TotalAmountOb <= 5000);
                        break;
                    case "9999":
                        query = query.Where(x => x.TotalAmountOb > 5000 && x.TotalAmountOb <= 10000);
                        break;
                    case "49999":
                        query = query.Where(x => x.TotalAmountOb > 10000 && x.TotalAmountOb <= 50000);
                        break;
                    case "99999":
                        query = query.Where(x => x.TotalAmountOb > 50000 && x.TotalAmountOb <= 100000);
                        break;
                    case "149999":
                        query = query.Where(x => x.TotalAmountOb > 100000 && x.TotalAmountOb <= 150000);
                        break;
                    case "199999":
                        query = query.Where(x => x.TotalAmountOb > 150000 && x.TotalAmountOb <= 200000);
                        break;
                    case "299999":
                        query = query.Where(x => x.TotalAmountOb > 200000 && x.TotalAmountOb <= 300000);
                        break;
                    case "499999":
                        query = query.Where(x => x.TotalAmountOb > 300000 && x.TotalAmountOb <= 500000);
                        break;
                    case "500000":
                        query = query.Where(x => x.TotalAmountOb > 500000);
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrEmpty(request.DebtorName))
                query = query.Where(x => x.Participants.Any(y => y.Name.ToLower().Contains(request.DebtorName.ToLower())));
            if (!string.IsNullOrEmpty(request.DebtorType))
                query = query.Where(x => x.Participants.Any(y => y.DebtorType == request.DebtorType));
            if (!string.IsNullOrEmpty(request.Region))
                query = query.Where(x => x.Participants.Any(y => y.Region.ToLower() == request.Region.ToLower()));
            if (request.ContractTypes != null && request.ContractTypes.Count() > 0)
            {
                var contractTypes = request.ContractTypes.Where(x => x.IsSelected).Select(x => x.Name.ToLower()).ToList();
                if (contractTypes.Count() > 0)
                {
                    query = query.Where(x => contractTypes.Contains(x.ContractType.ToLower()));
                }
            }
            if (request.ExcludedContractIds != null && request.ExcludedContractIds.Count() > 0 && reflectExcludedContractIds)
            {
                query = query.Where(x => !request.ExcludedContractIds.Contains(x.Id));
            }
            if (!string.IsNullOrEmpty(request.AddedInPortfolio))
            {
                if (request.AddedInPortfolio == "1")
                {
                    query = query.Where(x => x.PortfolioContracts.Any(x => x.Portfolio.OperationType == "SALE"));
                }
                if (request.AddedInPortfolio == "0")
                {
                    query = query.Where(x => !x.PortfolioContracts.Any(x => x.Portfolio.OperationType == "SALE"));
                }
            }

            return query.Include(x => x.Participants)
                .Include(x => x.Investors)
                .Include(x => x.Procedures)
                .Include(x => x.PortfolioContracts)
                .ToList();
        }
    }
}
