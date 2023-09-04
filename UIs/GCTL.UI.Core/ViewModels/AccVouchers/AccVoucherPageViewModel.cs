using GCTL.Core.ViewModels;
using GCTL.Core.ViewModels.AccVouchers;

namespace GCTL.UI.Core.ViewModels.AccVouchers
{
    public class AccVoucherPageViewModel : BaseViewModel
    {
        public AccVoucherSetupViewModel Setup { get; set; } = new AccVoucherSetupViewModel();
    }
}
