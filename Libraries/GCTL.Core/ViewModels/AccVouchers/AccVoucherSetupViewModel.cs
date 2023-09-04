using System.ComponentModel.DataAnnotations;

namespace GCTL.Core.ViewModels.AccVouchers
{
    public class AccVoucherSetupViewModel : BaseViewModel
    {       
       
        public decimal VoucherEntryAutoID { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Voucher Type")]
        public string VoucherType_Code { get; set; }
       
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Voucher Date")]
        public string VoucherDate { get; set; }
        
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Voucher No")]
        public string VoucherNo { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Narration")]
        public string Narration { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Company")]
        public string Main_CompanyCode { get; set; }

        public decimal? TotalAmount { get; set; }
        public string InvoiceNo { get; set; }
        public List<accVoucherEntryDetails> voucherDetails { get; set; } = new List<accVoucherEntryDetails>();

       
    }
}
