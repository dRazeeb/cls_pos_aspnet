using GCTL.Core.ViewModels;
using GCTL.Core.ViewModels.AccSubsidiaryLedgers;

namespace GCTL.UI.Core.ViewModels.AccSubsidiaryLedgers
{
    public class AccSubsidiaryLedgerPageViewModel : BaseViewModel
    {
        public AccSubsidiaryLedgerSetupViewModel Setup { get; set; } = new AccSubsidiaryLedgerSetupViewModel();
    }
}
