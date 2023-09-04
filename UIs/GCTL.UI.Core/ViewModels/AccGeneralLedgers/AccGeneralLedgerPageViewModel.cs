using GCTL.Core.ViewModels;
using GCTL.Core.ViewModels.AccGeneralLedgers;

namespace GCTL.UI.Core.ViewModels.AccGeneralLedgers
{
    public class AccGeneralLedgerPageViewModel : BaseViewModel
    {
        public AccGeneralLedgerSetupViewModel Setup { get; set; } = new AccGeneralLedgerSetupViewModel();
    }
}
