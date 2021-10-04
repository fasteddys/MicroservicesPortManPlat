using System;
using System.Collections.Generic;

#nullable disable

namespace PMaP.Models.DBModels
{
    public partial class Price
    {
        public int Id { get; set; }
        public int? PortfolioId { get; set; }
        public string Portfolio { get; set; }
        public string Subportfolio { get; set; }
        public DateTime? ProcessDate { get; set; }
        public int? ContractId { get; set; }
        public string Contract { get; set; }

        public virtual Contract ContractNavigation { get; set; }
        public virtual Portfolio PortfolioNavigation { get; set; }
    }
}
