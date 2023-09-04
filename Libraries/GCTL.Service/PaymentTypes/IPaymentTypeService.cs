using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;

namespace GCTL.Service.PaymentTypes
{
    public interface IPaymentTypeService
    {
        List<SalesDefPaymentType> GetPaymentTypes();
        SalesDefPaymentType GetPaymentType(string code); 
        bool DeletePaymentType(string id);    
        SalesDefPaymentType SavePaymentType(SalesDefPaymentType entity);
        bool IsPaymentTypeExistByCode(string code);
        bool IsPaymentTypeExist(string name);
        bool IsPaymentTypeExist(string name, string typeCode);
        IEnumerable<CommonSelectModel> PaymentTypeSelection();
    }
}