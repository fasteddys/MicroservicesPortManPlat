using System;
using System.Collections.Generic;

namespace PMaP.Models.PortfolioEvaluations
{
    public class PortfolioEvaluationRequest
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string DebtType { get; set; }
        public string Judicialized { get; set; }
        public string Insolvency { get; set; }
        public string PerformingStatus { get; set; }
        public string DebtOB { get; set; }
        public string DebtorName { get; set; }
        public string DebtorType { get; set; }
        public string Region { get; set; }
        public List<ContractType> ContractTypes { get; set; }
        public List<int> ExcludedContractIds { get; set; }
        public string AddedInPortfolio { get; set; }
        public bool ReflectExcludedContractIds { get; set; }
        public bool ExcludePossitiveOB { get; set; }
    }

    public class ContractType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
