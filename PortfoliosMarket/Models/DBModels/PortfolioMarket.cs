using System;
using System.Collections.Generic;

#nullable disable

namespace PortfoliosMarket.Models.DBModels
{
    public partial class PortfolioMarket
    {
        public PortfolioMarket()
        {
            Collaterals = new HashSet<Collateral>();
            ContractsNavigation = new HashSet<Contract>();
            Homes = new HashSet<Home>();
            Insolvencies = new HashSet<Insolvency>();
            Investors = new HashSet<Investor>();
            Participants = new HashSet<Participant>();
            PortfolioContracts = new HashSet<PortfolioContract>();
            PortfolioInvestors = new HashSet<PortfolioInvestor>();
            PortfolioParticipants = new HashSet<PortfolioParticipant>();
            Prices = new HashSet<Price>();
            Procedures = new HashSet<Procedure>();
        }
        public int Id { get; set; }
        public string Holder { get; set; }
        public string Project { get; set; }
        public string Investor { get; set; }
        public string Year { get; set; }
        public string Typology { get; set; }
        public string DebtType { get; set; }
        public string Value { get; set; }
        public byte? Status { get; set; }

        public virtual ICollection<Collateral> Collaterals { get; set; }
        public virtual ICollection<Contract> ContractsNavigation { get; set; }
        public virtual ICollection<Home> Homes { get; set; }
        public virtual ICollection<Insolvency> Insolvencies { get; set; }
        public virtual ICollection<Investor> Investors { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<PortfolioContract> PortfolioContracts { get; set; }
        public virtual ICollection<PortfolioInvestor> PortfolioInvestors { get; set; }
        public virtual ICollection<PortfolioParticipant> PortfolioParticipants { get; set; }
        public virtual ICollection<Price> Prices { get; set; }
        public virtual ICollection<Procedure> Procedures { get; set; }
    }
}
