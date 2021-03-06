using PMaP.Models.DBModels;
using System.Collections.Generic;

namespace PMaP.Models.Contracts
{
    public class ContractResponse
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public List<Contract> Contracts { get; set; }
        public Summary Summary { get; set; }
        public List<Participant> Participants { get; set; }
        public List<Procedure> Procedures { get; set; }
        public List<Investor> Investors { get; set; }
    }

    public class Summary
    {
        public int Contracts { get; set; }
        public decimal TotalOB { get; set; }
        public decimal SecuredOB { get; set; }
        public decimal UnsecuredOB { get; set; }
        public decimal SecuredPrice { get; set; }
        public decimal UnsecuredPrice { get; set; }
        public int Debtors { get; set; }
        public int Guarantors { get; set; }
    }
}
