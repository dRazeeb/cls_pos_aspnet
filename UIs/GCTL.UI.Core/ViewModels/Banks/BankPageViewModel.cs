using GCTL.Core.ViewModels;
using GCTL.Core.ViewModels.Banks;

namespace GCTL.UI.Core.ViewModels.Banks
{
    public class BankPageViewModel : BaseViewModel
    {
        public BankSetupViewModel Setup { get; set; } = new BankSetupViewModel();
    }
}
