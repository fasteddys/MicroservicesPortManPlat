using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioValuation.Models.ViewModels.Portfolio
{
    public class ViewModel
    {
        public int PortfolioId { get; set; }
        public string Portfolio { get; set; }
        public string Subportfolio { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateCutOff { get; set; }
        public DateTime? DateSigning { get; set; }
        public DateTime? DateClosing { get; set; }

        public string OperationType { get; set; }
        public string Situation { get; set; }
        public string OBCutOff { get; set; }
        public string OBSingning { get; set; }
        public string OBClosing { get; set; }
        public int? Contract { get; set; }
        public string Holder { get; set; }
    }
}
