using PMaP.Models.DBModels;
using System.Collections.Generic;

namespace PMaP.Models.PortfolioEvaluations
{
    public class UpdatePortfolioRequest
    {
        public List<Contract> Contracts { get; set; }
        public List<Participant> Participants { get; set; }
        public List<Investor> Investors { get; set; }

        public ViewModel ViewModel { get; set; }
    }
}
