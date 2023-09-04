using GCTL.Core.Data;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;

namespace GCTL.Service.Banks
{
    public class BankService : AppService<SalesDefBankInfo>, IBankService
    {
        private readonly IRepository<SalesDefBankInfo> bankRepository;

        public BankService(IRepository<SalesDefBankInfo> bankRepository)
            : base(bankRepository)
        {
            this.bankRepository = bankRepository;
        }

        public List<SalesDefBankInfo> GetBanks()
        {
            return GetAll();
        }

        public SalesDefBankInfo GetBank(string id)
        {
            return bankRepository.All().FirstOrDefault(x => x.BankId == id);
        }

        public SalesDefBankInfo SaveBank(SalesDefBankInfo entity)
        {
            if (IsBankExistByCode(entity.BankId))
                Update(entity);
            else
                Add(entity);

            return entity;
        }

        public bool DeleteBank(string id)
        {
            var entity = GetBank(id);
            if (entity != null)
            {
                bankRepository.Delete(entity);
                return true;
            }
            return false;
        }

        public bool IsBankExistByCode(string code)
        {
            return bankRepository.All().Any(x => x.BankId == code);
        }

        public bool IsBankExist(string name)
        {
            return bankRepository.All().Any(x => x.BankName == name);
        }

        public bool IsBankExist(string name, string typeCode)
        {
            return bankRepository.All().Any(x => x.BankName == name && x.BankId != typeCode);
        }

        public IEnumerable<CommonSelectModel> BankSelection()
        {
            return bankRepository.All()
                .Select(x => new CommonSelectModel
                {
                    Code = x.BankId,
                    Name = x.BankName
                });
        }
    }
}
