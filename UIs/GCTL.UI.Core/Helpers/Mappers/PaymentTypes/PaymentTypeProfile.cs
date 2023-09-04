using GCTL.Data.Models;
using AutoMapper;
using GCTL.Core.ViewModels.PaymentTypes;

namespace GCTL.UI.Core.Helpers.Mappers.PaymentTypes
{
    public class PaymentTypeProfile : Profile
    {
        public PaymentTypeProfile()
        {
            CreateMap<SalesDefPaymentType, PaymentTypeSetupViewModel>().ReverseMap();
        }
    }
}
