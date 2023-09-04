using GCTL.Core.Data;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;

namespace GCTL.Service.PaymentTypes
{
    public class PaymentTypeService : AppService<SalesDefPaymentType>, IPaymentTypeService
    {
        private readonly IRepository<SalesDefPaymentType> paymentTypeRepository;

        public PaymentTypeService(IRepository<SalesDefPaymentType> paymentTypeRepository)
            : base(paymentTypeRepository)
        {
            this.paymentTypeRepository = paymentTypeRepository;
        }

        public List<SalesDefPaymentType> GetPaymentTypes()
        {
            return GetAll();
        }

        public SalesDefPaymentType GetPaymentType(string id)
        {
            return paymentTypeRepository.GetById(id);
        }

        public SalesDefPaymentType SavePaymentType(SalesDefPaymentType entity)
        {
            if (IsPaymentTypeExistByCode(entity.PaymentTypeId))
                Update(entity);
            else
                Add(entity);

            return entity;
        }

        public bool DeletePaymentType(string id)
        {
            var entity = GetPaymentType(id);
            if (entity != null)
            {
                paymentTypeRepository.Delete(entity);
                return true;
            }
            return false;
        }

        public bool IsPaymentTypeExistByCode(string code)
        {
            return paymentTypeRepository.All().Any(x => x.PaymentTypeId == code);
        }

        public bool IsPaymentTypeExist(string name)
        {
            return paymentTypeRepository.All().Any(x => x.PaymentType == name);
        }

        public bool IsPaymentTypeExist(string name, string typeCode)
        {
            return paymentTypeRepository.All().Any(x => x.PaymentType == name && x.PaymentTypeId != typeCode);
        }

        public IEnumerable<CommonSelectModel> PaymentTypeSelection()
        {
            return paymentTypeRepository.All()
                .Select(x => new CommonSelectModel
                {
                    Code = x.PaymentTypeId,
                    Name = x.PaymentType
                });
        }
    }
}
