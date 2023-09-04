using System.ComponentModel.DataAnnotations;

namespace GCTL.Core.ViewModels.ExpenseEntry
{
    public class ExpenseEntrySetupViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "{0} is required.")]
        public string ExpenseCode { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        public string ExpenseDate { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        public string SubSusidiaryLedgerCodeNo { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        public decimal? Amount { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        public string PaymentMode { get; set; }
        public string BkashNo { get; set; }
        public string ChequeNo { get; set; }
        public string ChequeDate { get; set; }
        public string BankAccountNo { get; set; }
        public string TransferBankFrom { get; set; }
        public string TransferBankTo { get; set; }
        public string Remarks { get; set; }

    }
}
