using System.ComponentModel.DataAnnotations;

namespace GCTL.Core.ViewModels.Banks
{
    public class BankSetupViewModel : BaseViewModel
    {
        public int AutoId { get; set; }

        [Display(Name = "Bank Id")]
        public string BankId { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Short Name")]
        public string ShortName { get; set; }
    }
}
