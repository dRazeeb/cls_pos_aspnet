using GCTL.Core.ViewModels;
using GCTL.Core.ViewModels.AccVoucherTypes;

namespace GCTL.UI.Core.ViewModels.AccVoucherTypes
{
    public class AccVoucherTypePageViewModel : BaseViewModel
    {
        public AccVoucherTypeSetupViewModel Setup { get; set; } = new AccVoucherTypeSetupViewModel();
    }
}
