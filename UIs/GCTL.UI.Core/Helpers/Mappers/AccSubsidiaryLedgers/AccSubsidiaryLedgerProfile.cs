using AutoMapper;
using GCTL.Core.ViewModels.AccSubsidiaryLedgers;
using GCTL.Data.Models;

namespace GCTL.UI.Core.Helpers.Mappers.AccSubsidiaryLedgers
{
    public class AccSubsidiaryLedgerProfile : Profile
    {
        public AccSubsidiaryLedgerProfile()
        {
            CreateMap<AccSubsidiaryLedger, AccSubsidiaryLedgerSetupViewModel>().ReverseMap();
        }
    }
}
