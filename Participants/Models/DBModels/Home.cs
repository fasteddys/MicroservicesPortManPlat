using System;
using System.Collections.Generic;

#nullable disable

namespace Participants.Models.DBModels
{
    public partial class Home
    {
        public int IdHome { get; set; }
        public int? PortfolioId { get; set; }
        public string Portfolio { get; set; }
        public string Subportfolio { get; set; }
        public decimal? NominalValue { get; set; }
        public decimal? Secured { get; set; }
        public decimal? Unsecured { get; set; }
        public string DebtType { get; set; }
        public decimal? PriceSecured { get; set; }
        public decimal? UnsecuredPrice { get; set; }
        public string AssignmentEntity { get; set; }
        public int? AcquisitionYear { get; set; }

        public virtual Portfolio PortfolioNavigation { get; set; }
    }
}
