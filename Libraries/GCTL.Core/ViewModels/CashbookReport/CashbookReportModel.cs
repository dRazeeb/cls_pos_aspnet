using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Core.ViewModels.CashbookReport
{
    public class CashbookReportModel
    {
        public string rVoucherNo { get; set; }
        public string rNarration { get; set; }
        public decimal rCashinHand { get; set; }
        public decimal rCashatBank { get; set; }
        public decimal rBKash { get; set; }

        public string pVoucherNo { get; set; }
        public string pNarration { get; set; }
        public decimal pCashinHand { get; set; }
        public decimal pCashatBank { get; set; }
        public decimal pBKash { get; set; }
        public string SearchFromDate { get; set; }
        public string SearchToDate { get; set; }
    }
}
