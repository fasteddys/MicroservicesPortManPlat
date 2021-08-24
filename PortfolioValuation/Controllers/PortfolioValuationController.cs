using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

            return response;
        }

        [HttpPost("portfolio")]
        public async Task<Response> AddPortfolio(AddPortfolioRequest request)
        {
            Response response = new Response();

            try
            {
                //request.Contracts.ForEach(x => x.Id = 0);
                //request.Contracts.ForEach(x => x.PortfolioId = null);
                //request.Contracts.ForEach(x => x.ProcessDate = Convert.ToDateTime(request.ViewModel.DateAdded));

                //request.Contracts.ForEach(x => x.Participants.ToList().ForEach(x => x.Id = 0));
                //request.Contracts.ForEach(x => x.Participants.ToList().ForEach(x => x.PortfolioId = null));
                //request.Contracts.ForEach(x => x.Participants.ToList().ForEach(x => x.ContractId = 0));
                //request.Contracts.ForEach(x => x.Participants.ToList().ForEach(x => x.ProcessDate = Convert.ToDateTime(request.ViewModel.DateAdded)));

                //request.Contracts.ForEach(x => x.Investors.ToList().ForEach(x => x.Id = 0));
                //request.Contracts.ForEach(x => x.Investors.ToList().ForEach(x => x.PortfolioId = null));
                //request.Contracts.ForEach(x => x.Investors.ToList().ForEach(x => x.ContractId = 0));

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
                    //Contracts = request.Contracts
                    PortfolioContracts = portfolioContracts,
                    PortfolioInvestors = portfolioInvestors,
                    PortfolioParticipants = portfolioParticipants
                };
                //if (!string.IsNullOrEmpty(request.ViewModel.DateClosing))
                //    portfolio.ClosingDate = Convert.ToDateTime(request.ViewModel.DateClosing);
                //if (!string.IsNullOrEmpty(request.ViewModel.DateAdded))
                //    portfolio.CreationDate = Convert.ToDateTime(request.ViewModel.DateAdded);
                //if (!string.IsNullOrEmpty(request.ViewModel.DateCutOff))
                //    portfolio.CutOffDate = Convert.ToDateTime(request.ViewModel.DateCutOff);
                //if (!string.IsNullOrEmpty(request.ViewModel.DateSigning))
                //    portfolio.SigningDate = Convert.ToDateTime(request.ViewModel.DateSigning);

                _pmpContext.Add(portfolio);
                await _pmpContext.SaveChangesAsync();

                //var portfolioId = portfolio.Id;

                //portfolio.Contracts.ToList().ForEach(x => x.Participants.ToList().ForEach(x => x.PortfolioId = portfolioId));
                //portfolio.Contracts.ToList().ForEach(x => x.Investors.ToList().ForEach(x => x.PortfolioId = portfolioId));

                //_pmpContext.Update(portfolio);
                //await _pmpContext.SaveChangesAsync();

                response.ResponseCode = 200;
                response.Message = "Success";
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

                var portfolioContractsDelete = _pmpContext.PortfolioContracts.Where(x => x.PortfolioId == portfolio.Id).ToList();
                _pmpContext.RemoveRange(portfolioContractsDelete);

                var portfolioInvestorsDelete = _pmpContext.PortfolioInvestors.Where(x => x.PortfolioId == portfolio.Id).ToList();
                _pmpContext.RemoveRange(portfolioInvestorsDelete);

                var portfolioParticipantsDelete = _pmpContext.PortfolioParticipants.Where(x => x.PortfolioId == portfolio.Id).ToList();
                _pmpContext.RemoveRange(portfolioParticipantsDelete);

                await _pmpContext.SaveChangesAsync();

                List<Investor> investors = new List<Investor>();
                List<Participant> participants = new List<Participant>();
                foreach (var item in request.Contracts)
                {
                    investors.AddRange(item.Investors);
                    participants.AddRange(item.Participants);
                }

                List<PortfolioContract> portfolioContracts = new List<PortfolioContract>();
                var contractIds = request.Contracts.Select(x => x.Id).ToList();
                foreach (var contractId in contractIds)
                {
                    portfolioContracts.Add(new PortfolioContract { ContractId = contractId, PortfolioId = portfolio.Id });
                }

                List<PortfolioInvestor> portfolioInvestors = new List<PortfolioInvestor>();
                var investorIds = investors.Select(x => x.Id).ToList();
                foreach (var investorId in investorIds)
                {
                    portfolioInvestors.Add(new PortfolioInvestor { InvestorId = investorId, PortfolioId = portfolio.Id });
                }

                List<PortfolioParticipant> portfolioParticipants = new List<PortfolioParticipant>();
                var participantIds = participants.Select(x => x.Id).ToList();
                foreach (var participantId in participantIds)
                {
                    portfolioParticipants.Add(new PortfolioParticipant { ParticipantId = participantId, PortfolioId = portfolio.Id });
                }

                portfolio.PortfolioContracts = portfolioContracts;
                portfolio.PortfolioInvestors = portfolioInvestors;
                portfolio.PortfolioParticipants = portfolioParticipants;

                _pmpContext.Update(portfolio);
                await _pmpContext.SaveChangesAsync();

                //var requestContractIds = request.Contracts.Select(x => x.Id).ToList();
                //var portfolioContractsToDelete = portfolioContracts.Where(x => !requestContractIds.Contains(x.Id)).ToList();
                //_pmpContext.RemoveRange(portfolioContractsToDelete);
                //await _pmpContext.SaveChangesAsync();

                //var portfolioContext = _pmpContext.Portfolios.Where(x => x.Id == portfolioId)
                //    .Include(x => x.Contracts)
                //    .Include(x => x.Participants)
                //    .Include(x => x.Procedures)
                //    .Include(x => x.Investors)
                //    .FirstOrDefault();
                //if (portfolioContext != null)
                //{
                //    var existingContractIds = portfolioContext.Contracts.Select(x => x.Id).ToList();
                //    if (existingContractIds.Count() > 0)
                //    {
                //        request.Contracts.Where(x => !existingContractIds.Contains(x.Id)).ToList().ForEach(x => x.Participants.ToList().ForEach(x => x.Id = 0));
                //        request.Contracts.Where(x => !existingContractIds.Contains(x.Id)).ToList().ForEach(x => x.Participants.ToList().ForEach(x => x.PortfolioId = portfolioId));
                //        request.Contracts.Where(x => !existingContractIds.Contains(x.Id)).ToList().ForEach(x => x.Participants.ToList().ForEach(x => x.ContractId = 0));

                //        request.Contracts.Where(x => !existingContractIds.Contains(x.Id)).ToList().ForEach(x => x.Investors.ToList().ForEach(x => x.Id = 0));
                //        request.Contracts.Where(x => !existingContractIds.Contains(x.Id)).ToList().ForEach(x => x.Investors.ToList().ForEach(x => x.PortfolioId = portfolioId));
                //        request.Contracts.Where(x => !existingContractIds.Contains(x.Id)).ToList().ForEach(x => x.Investors.ToList().ForEach(x => x.ContractId = 0));

                //        request.Contracts.Where(x => !existingContractIds.Contains(x.Id)).ToList().ForEach(x => x.Id = 0);
                //        request.Contracts.Where(x => !existingContractIds.Contains(x.Id)).ToList().ForEach(x => x.PortfolioId = portfolioId);
                //    }

                //    portfolioContext.Portfolio1 = request.ViewModel.PortfolioValuationAdd.Portfolio;
                //    portfolioContext.Subportfolio = request.ViewModel.PortfolioValuationAdd.Subportfolio;
                //    portfolioContext.Contracts = request.Contracts;

                //    _pmpContext.Update(portfolioContext);
                //    await _pmpContext.SaveChangesAsync();
                //}

                response.ResponseCode = 200;
                response.Message = "Success";
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
                response.Message = "Success";
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
                var contractTypes = request.ContractTypes.Where(x => x.IsSelected).Select(x => x.Name).ToList();
                if (contractTypes.Count() > 0)
                {
                    query = query.Where(x => contractTypes.Contains(x.ContractType));
                }
            }
            if (request.ExcludedContractIds != null && request.ExcludedContractIds.Count() > 0)
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
