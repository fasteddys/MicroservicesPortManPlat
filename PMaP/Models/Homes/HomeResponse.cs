using PMaP.Models.DBModels;
using System.Collections.Generic;

namespace PMaP.Models.Homes
{
    public class HomeResponse
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<Home> Documents { get; set; }
    }
}
