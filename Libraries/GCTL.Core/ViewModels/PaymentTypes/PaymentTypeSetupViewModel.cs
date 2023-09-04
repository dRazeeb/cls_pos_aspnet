using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace GCTL.Core.ViewModels.PaymentTypes
{
    public class PaymentTypeSetupViewModel : BaseViewModel
    {
        public int Tc { get; set; }
        public string PaymentTypeId { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string PaymentType { get; set; }
        public string ShortName { get; set; }
        public string UnitTypeId { get; set; }
        public dynamic UnitTypes { get; set; }
    }
}
