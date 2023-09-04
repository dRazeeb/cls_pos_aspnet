using System.ComponentModel.DataAnnotations;

namespace GCTL.Core.ViewModels.BankBranches
{
    public class BankBranchSetupViewModel : BaseViewModel
    {
        public int AutoId { get; set; }

        [Display(Name = "Bank Branch Id")]
        public string BankBranchId { get; set; }

        [Display(Name = "Bank Id")]
        public string BankId { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Bank Branch Name")]
        public string BankBranchName { get; set; }

        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        [Display(Name = "Swift Code")]
        public string Swiftcode { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
