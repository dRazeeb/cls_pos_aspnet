using GCTL.Core.ViewModels;
using GCTL.Core.ViewModels.AccSubSubsidiaryLedgers;

namespace GCTL.UI.Core.ViewModels.AccSubSubsidiaryLedgers
{
    public class AccSubSubsidiaryLedgerPageViewModel : BaseViewModel
    {
        public AccSubSubsidiaryLedgerSetupViewModel Setup { get; set; } = new AccSubSubsidiaryLedgerSetupViewModel();
    }
}
