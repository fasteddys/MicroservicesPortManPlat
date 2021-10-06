using PortfolioValuation.Models.DBModels;
using PortfolioValuation.Models.ViewModels.Portfolio;
using System.Collections.Generic;

namespace PortfolioValuation.Models
{
    public class AddPortfolioRequest
    {
        public List<Contract> Contracts { get; set; }
        public List<Participant> Participants { get; set; }
        public List<Investor> Investors { get; set; }
        public List<Procedure> Procedures { get; set; }

        public ViewModel ViewModel { get; set; }
    }
}
