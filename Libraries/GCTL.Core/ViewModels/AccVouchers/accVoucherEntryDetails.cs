

namespace GCTL.Core.ViewModels.AccVouchers
{
    public class accVoucherEntryDetails : BaseViewModel
    {
        public decimal VoucherEntryAutoID { get; set; }
        public string VoucherEntryDetailsCodeNo { get; set; }
        public string AccCode { get; set; }
        public string TrType { get; set; }
        public string Description { get; set; }
        public decimal? DebitAmount { get; set; }
        public decimal? CreditAmount { get; set; }
        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }
    }
}
