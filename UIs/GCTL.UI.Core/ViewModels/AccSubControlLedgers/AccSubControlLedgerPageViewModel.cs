using GCTL.Core.ViewModels;
using GCTL.Core.ViewModels.AccSubControlLedgers;

namespace GCTL.UI.Core.ViewModels.AccSubControlLedgers
{
    public class AccSubControlLedgerPageViewModel : BaseViewModel
    {
        public AccSubControlLedgerSetupViewModel Setup { get; set; } = new AccSubControlLedgerSetupViewModel();
    }
}

