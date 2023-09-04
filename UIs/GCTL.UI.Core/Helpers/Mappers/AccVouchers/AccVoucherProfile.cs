using AutoMapper;
using GCTL.Core.ViewModels.AccVouchers;
using GCTL.Data.Models;

namespace GCTL.UI.Core.Helpers.Mappers.AccVouchers
{
    public class AccVoucherProfile : Profile
    {
        public AccVoucherProfile()
        {
            CreateMap<AccVoucherEntry, AccVoucherSetupViewModel>().ReverseMap();
        }
    }
}
