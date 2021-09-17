using System;
using System.Collections.Generic;

#nullable disable

namespace PortfoliosMarket.Models.DBModels
{
    public partial class Investor
    {
        public Investor()
        {
            PortfolioInvestors = new HashSet<PortfolioInvestor>();
        }

        public int Id { get; set; }
        public int? PortfolioId { get; set; }
        public int? ContractId { get; set; }
        public string InvestorName { get; set; }
        public string SocialAddress { get; set; }
        public string TaxIdentification { get; set; }
        public string Mail { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Iban { get; set; }
        public string Bank { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual Portfolio Portfolio { get; set; }
        public virtual ICollection<PortfolioInvestor> PortfolioInvestors { get; set; }
    }
}
