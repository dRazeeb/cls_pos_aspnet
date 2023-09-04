using GCTL.Data.Models;
using AutoMapper;
using GCTL.Core.ViewModels.AccControlLedger;

namespace GCTL.UI.Core.Helpers.Mappers.ControlLedger
{
    public class AccControlLedgerProfile:Profile
    {
        public AccControlLedgerProfile()
        {
            CreateMap<AccControlLedger, AccControlLedgerSetupViewModel>().ReverseMap();
        }
    }
}
