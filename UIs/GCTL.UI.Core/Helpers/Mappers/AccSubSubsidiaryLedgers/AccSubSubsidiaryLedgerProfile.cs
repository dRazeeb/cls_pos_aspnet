using AutoMapper;
using GCTL.Core.ViewModels.AccSubSubsidiaryLedgers;
using GCTL.Data.Models;

namespace GCTL.UI.Core.Helpers.Mappers.AccSubSubsidiaryLedgers
{
    public class AccSubSubsidiaryLedgerProfile : Profile
    {
        public AccSubSubsidiaryLedgerProfile()
        {
            CreateMap<AccSubSubsidiaryLedger, AccSubSubsidiaryLedgerSetupViewModel>().ReverseMap();
        }
    }
}
