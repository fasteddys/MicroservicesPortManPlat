using System.Collections.Generic;

namespace Home.Models
{
    public class Response
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<DBModels.Home> Documents { get; set; }
    }
}
