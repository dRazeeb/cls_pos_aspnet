using System.ComponentModel.DataAnnotations;

namespace GCTL.Core.ViewModels.BankAccounts
{
    public class BankAccountSetupViewModel : BaseViewModel
    {
        public int AutoId { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Bank Account Id")]
        public string AccInfoId { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Bank")]
        public string BankId { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Bank Branch")]
        public string BranchId { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Account No.")]
        public string AccountNo { get; set; }
    }
}
