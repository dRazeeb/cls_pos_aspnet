using GCTL.Core.ViewModels;
using GCTL.Core.ViewModels.BankBranches;

namespace GCTL.UI.Core.ViewModels.BankBranches
{
    public class BankBranchPageViewModel : BaseViewModel
    {
        public BankBranchSetupViewModel Setup { get; set; } = new BankBranchSetupViewModel();
    }
}
