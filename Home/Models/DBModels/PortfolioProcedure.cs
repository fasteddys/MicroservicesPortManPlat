using System;
using System.Collections.Generic;

#nullable disable

namespace Home.Models.DBModels
{
    public partial class PortfolioProcedure
    {
        public int Id { get; set; }
        public int? PortfolioId { get; set; }
        public int? ProcedureId { get; set; }

        public virtual Portfolio Portfolio { get; set; }
        public virtual Procedure Procedure { get; set; }
    }
}
