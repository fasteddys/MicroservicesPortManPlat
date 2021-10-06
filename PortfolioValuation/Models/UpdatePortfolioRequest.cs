using PortfolioValuation.Models.DBModels;
using PortfolioValuation.Models.ViewModels.PortfolioValuation;
using System.Collections.Generic;

namespace PortfolioValuation.Models
{
    public class UpdatePortfolioRequest
    {
        public List<Contract> Contracts { get; set; }
        public List<Participant> Participants { get; set; }
        public List<Investor> Investors { get; set; }

        public ViewModel ViewModel { get; set; }
    }
}
