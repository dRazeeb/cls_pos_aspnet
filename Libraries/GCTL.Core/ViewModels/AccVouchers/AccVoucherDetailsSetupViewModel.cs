using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Core.ViewModels.AccVouchers
{
    public class AccVoucherDetailsSetupViewModel
    {
        public decimal autoId { get; set; }
        public decimal VoucherEntryAutoID { get; set; }
  
        public string AccCode { get; set; }
        public string TrType { get; set; }
        public string Description { get; set; }
        public decimal? DebitAmount { get; set; }
        public decimal? CreditAmount { get; set; }

        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }
        public string AccountHead { get; set; }
    }
}
