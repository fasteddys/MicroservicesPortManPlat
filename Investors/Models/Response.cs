using Investors.Models.DBModels;
using System.Collections.Generic;

namespace Investors.Models
{
    public class Response
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<Investor> Investors { get; set; }
    }
}
