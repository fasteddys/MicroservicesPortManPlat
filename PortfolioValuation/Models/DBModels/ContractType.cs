using System;
using System.Collections.Generic;

#nullable disable

namespace PortfolioValuation.Models.DBModels
{
    public partial class ContractType
    {
        public ContractType()
        {
            Contracts = new HashSet<Contract>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; }
    }
}
