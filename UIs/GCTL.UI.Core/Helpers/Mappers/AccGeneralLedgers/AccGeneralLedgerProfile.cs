using AutoMapper;
using GCTL.Core.ViewModels.AccGeneralLedgers;
using GCTL.Data.Models;

namespace GCTL.UI.Core.Helpers.Mappers.AccGeneralLedgers
{
    public class AccGeneralLedgerProfile : Profile
    {
        public AccGeneralLedgerProfile()
        {
            CreateMap<AccGeneralLedger, AccGeneralLedgerSetupViewModel>().ReverseMap();
        }
    }
}
