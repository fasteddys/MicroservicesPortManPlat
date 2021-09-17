using PortfoliosMarket.Models.DBModels;
using System.Collections.Generic;

namespace Portfolios.Models
{
    public class Response
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<PortfolioMarket> Documents { get; set; }
    }
}
