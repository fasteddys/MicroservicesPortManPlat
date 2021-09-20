using Participants.Models.DBModels;
using System.Collections.Generic;

namespace Participants.Models
{
    public class Response
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<Participant> Participants { get; set; }
    }
}
