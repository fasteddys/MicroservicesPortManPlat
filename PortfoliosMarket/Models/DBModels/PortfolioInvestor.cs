using System;
using System.Collections.Generic;

#nullable disable

namespace PortfoliosMarket.Models.DBModels
{
    public partial class PortfolioInvestor
    {
        public int Id { get; set; }
        public int? PortfolioId { get; set; }
        public int? InvestorId { get; set; }

        public virtual Investor Investor { get; set; }
        public virtual Portfolio Portfolio { get; set; }
    }
}
