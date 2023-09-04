using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Core.ViewModels.AccVouchers
{
    public class VoucherEntryReportModel
    {
        public string VoucherEntryCodeNo { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public string Narration { get; set; }
        public string Description { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal TotalDebitAmount { get; set; }

        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string CreatedBy { get; set; }
        public string VoucherType { get; set; }
        public string NumberInEnglish { get; set; }
        public string CheckedBy { get; set; }
        public string ApprovedBy { get; set; }
    }
}
