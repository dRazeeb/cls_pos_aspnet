using GCTL.Data.Models;
using AutoMapper;
using GCTL.Core.ViewModels.BankBranches;

namespace GCTL.UI.Core.Helpers.Mappers.BankBranches
{
    public class BankBranchProfile : Profile
    {
        public BankBranchProfile()
        {
            CreateMap<SalesDefBankBranchInfo, BankBranchSetupViewModel>().ReverseMap();
        }
    }
}
