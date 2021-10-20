using System;
using System.Collections.Generic;

#nullable disable

namespace PortfoliosMarket.Models.DBModels
{
    public partial class PortfolioMarket
    {
        public int Id { get; set; }
        public string Holder { get; set; }
        public string HolderLogo { get; set; }
        public string Project { get; set; }
        public string Investor { get; set; }
        public string InvestorLogo { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
        public string Typology { get; set; }
        public string DebtType { get; set; }
        public decimal? Value { get; set; }
        public byte? Status { get; set; }
    }
}
