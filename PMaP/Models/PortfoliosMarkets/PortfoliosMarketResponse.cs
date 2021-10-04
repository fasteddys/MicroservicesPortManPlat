using PMaP.Models.DBModels;
using System.Collections.Generic;

namespace PMaP.Models.PortfoliosMarkets
{
    public class PortfoliosMarketResponse
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<PortfolioMarket> PortfolioMarkets { get; set; }
    }
}
