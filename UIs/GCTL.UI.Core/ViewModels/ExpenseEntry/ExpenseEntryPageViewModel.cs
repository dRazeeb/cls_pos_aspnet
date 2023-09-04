using GCTL.Core.ViewModels;
using GCTL.Core.ViewModels.ExpenseEntry;

namespace GCTL.UI.Core.ViewModels.ExpenseEntry
{
    public class ExpenseEntryPageViewModel : BaseViewModel
    {
        //public DoctorWorkingPlaceSetupViewModel Setup { get; set; } = new DoctorWorkingPlaceSetupViewModel();
        public ExpenseEntrySetupViewModel Setup { get; set; } = new ExpenseEntrySetupViewModel();

    }
}
