using GCTL.Data.Models;
using AutoMapper;
using GCTL.Core.ViewModels.AccSubControlLedgers;

namespace GCTL.UI.Core.Helpers.Mappers.AccSubControlLedgers
{
    public class AccSubControlLedgerProfile : Profile
    {
        public AccSubControlLedgerProfile()
        {
            CreateMap<AccSubControlLedger, AccSubControlLedgerSetupViewModel>().ReverseMap();
        }
    }
}
