using GCTL.Core.ViewModels;
using GCTL.Core.ViewModels.Navigations;

namespace GCTL.UI.Core.ViewModels.Navigations
{
    public class NavigationPageViewModel : BaseViewModel
    {
        public NavigationSetupViewModel Setup { get; set; } = new NavigationSetupViewModel();
    }
}
