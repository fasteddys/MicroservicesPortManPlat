using System;
using System.Collections.Generic;

#nullable disable

namespace Home.Models.DBModels
{
    public partial class Portfolio
    {
        public Portfolio()
        {
            Collaterals = new HashSet<Collateral>();
            Contracts = new HashSet<Contract>();
            Homes = new HashSet<Home>();
            Insolvencies = new HashSet<Insolvency>();
            Participants = new HashSet<Participant>();
            Prices = new HashSet<Price>();
            Procedures = new HashSet<Procedure>();
        }

        public int Id { get; set; }
        public string Portfolio1 { get; set; }
        public string Subportfolio { get; set; }
        public string OperationType { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? CutOffDate { get; set; }
        public DateTime? SigningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public decimal? CutOffOb { get; set; }
        public decimal? SigningOb { get; set; }
        public decimal? ClosingOb { get; set; }
        public string Investor { get; set; }

        public virtual ICollection<Collateral> Collaterals { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<Home> Homes { get; set; }
        public virtual ICollection<Insolvency> Insolvencies { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<Price> Prices { get; set; }
        public virtual ICollection<Procedure> Procedures { get; set; }
    }
}
