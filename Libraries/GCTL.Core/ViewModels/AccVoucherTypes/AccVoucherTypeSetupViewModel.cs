using System.ComponentModel.DataAnnotations;

namespace GCTL.Core.ViewModels.AccVoucherTypes
{
    public class AccVoucherTypeSetupViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "{0} is required.")]
        public string VoucherType_Code { get; set; }

        [Required(ErrorMessage = "{0} is required.")]       
        public string Voucher_TypeName { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string Description { get; set; }
    }
}
