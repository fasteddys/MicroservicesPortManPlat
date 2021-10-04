using PMaP.Models.DBModels;
using System.Collections.Generic;

namespace PMaP.Models.Portfolios
{
    public class PortfolioResponse
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<Portfolio> Portfolios { get; set; }
    }
}
