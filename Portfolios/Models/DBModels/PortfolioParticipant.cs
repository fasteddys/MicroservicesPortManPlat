using System;
using System.Collections.Generic;

#nullable disable

namespace Portfolios.Models.DBModels
{
    public partial class PortfolioParticipant
    {
        public int Id { get; set; }
        public int? PortfolioId { get; set; }
        public int? ParticipantId { get; set; }

        public virtual Participant Participant { get; set; }
        public virtual Portfolio Portfolio { get; set; }
    }
}
