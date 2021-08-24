using Procedures.Models.DBModels;
using System.Collections.Generic;

namespace Procedures.Models
{
    public class Response
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public List<Procedure> Procedures { get; set; }
    }
}
