using GCTL.Core.ViewModels;
using GCTL.Core.ViewModels.AccControlLedger;

namespace GCTL.UI.Core.ViewModels.AccControlLedger
{
    public class AccControlLedgerPageViewModel : BaseViewModel
    {
        public AccControlLedgerSetupViewModel Setup { get; set; } = new AccControlLedgerSetupViewModel();
    }
}
