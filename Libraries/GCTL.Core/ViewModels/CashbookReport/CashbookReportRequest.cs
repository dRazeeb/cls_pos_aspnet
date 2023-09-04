using GCTL.Core.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Core.ViewModels.CashbookReport
{
    public class CashbookReportRequest : ApplicationReportRequest
    {
        public string[] BillTypeId { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }
}
