using System;
using System.Collections.Generic;

#nullable disable

namespace PortfoliosMarket.Models.DBModels
{
    public partial class PortfolioContract
    {
        public int Id { get; set; }
        public int? PortfolioId { get; set; }
        public int? ContractId { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual Portfolio Portfolio { get; set; }
    }
}
