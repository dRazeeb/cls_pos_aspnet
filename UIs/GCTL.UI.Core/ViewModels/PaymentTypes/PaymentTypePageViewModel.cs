using GCTL.Core.ViewModels;
using GCTL.Core.ViewModels.PaymentTypes;

namespace GCTL.UI.Core.ViewModels.PaymentTypes
{
    public class PaymentTypePageViewModel : BaseViewModel
    {
        public PaymentTypeSetupViewModel Setup { get; set; } = new PaymentTypeSetupViewModel();
    }
}
