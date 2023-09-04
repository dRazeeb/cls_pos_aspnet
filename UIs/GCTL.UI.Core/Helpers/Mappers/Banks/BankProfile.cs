using GCTL.Data.Models;
using AutoMapper;
using GCTL.Core.ViewModels.Banks;

namespace GCTL.UI.Core.Helpers.Mappers.Banks
{
    public class BankProfile : Profile
    {
        public BankProfile()
        {
            CreateMap<SalesDefBankInfo, BankSetupViewModel>().ReverseMap();
        }
    }
}
