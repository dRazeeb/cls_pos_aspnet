using GCTL.Data.Models;
using AutoMapper;
using GCTL.Core.ViewModels.ExpenseEntry;

namespace GCTL.UI.Core.Helpers.Mappers.ExpenseEntry
{
    public class ExpenseEntryProfile : Profile
    {
        public ExpenseEntryProfile()
        {
            CreateMap<HmsExpenseEntry, ExpenseEntrySetupViewModel>().ReverseMap();
        }
    }
}
