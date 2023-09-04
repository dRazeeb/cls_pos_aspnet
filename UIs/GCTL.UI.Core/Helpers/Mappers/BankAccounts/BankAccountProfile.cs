using GCTL.Data.Models;
using AutoMapper;
using GCTL.Core.ViewModels.BankAccounts;

namespace GCTL.UI.Core.Helpers.Mappers.BankAccounts
{
    public class BankAccountProfile : Profile
    {
        public BankAccountProfile()
        {
            CreateMap<CoreBankAccountInformation, BankAccountSetupViewModel>().ReverseMap();
        }
    }
}
