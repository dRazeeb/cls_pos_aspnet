using GCTL.Core.ViewModels;
using GCTL.Core.ViewModels.BankAccounts;

namespace GCTL.UI.Core.ViewModels.BankAccounts
{
    public class BankAccountPageViewModel : BaseViewModel
    {
        public BankAccountSetupViewModel Setup { get; set; } = new BankAccountSetupViewModel();
    }
}
