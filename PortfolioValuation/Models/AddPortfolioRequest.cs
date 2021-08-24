using PortfolioValuation.Models.DBModels;
using PortfolioValuation.Models.ViewModels.Portfolio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioValuation.Models
{
    public class AddPortfolioRequest
    {
        public List<Contract> Contracts { get; set; }
        public List<Participant> Participants { get; set; }
        public List<Investor> Investors { get; set; }

        public ViewModel ViewModel { get; set; }
    }
}
