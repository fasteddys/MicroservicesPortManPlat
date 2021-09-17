using System;
using System.Collections.Generic;

#nullable disable

namespace Contracts.Models.DBModels
{
    public partial class PortfolioMarket
    {
        public int Id { get; set; }
        public string Holder { get; set; }
        public string Project { get; set; }
        public string Investor { get; set; }
        public string Year { get; set; }
        public string Typology { get; set; }
        public string DebtType { get; set; }
        public string Value { get; set; }
        public byte? Status { get; set; }
    }
}
