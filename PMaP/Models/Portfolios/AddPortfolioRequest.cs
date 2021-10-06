using PMaP.Models.DBModels;
using System.Collections.Generic;

namespace PMaP.Models.Portfolios
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
