using GCTL.Core.Data;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;

namespace GCTL.Service.BankBranches
{
    public class BankBranchService : AppService<SalesDefBankBranchInfo>, IBankBranchService
    {
        private readonly IRepository<SalesDefBankBranchInfo> bankBranchRepository;

        public BankBranchService(IRepository<SalesDefBankBranchInfo> bankBranchRepository)
            : base(bankBranchRepository)
        {
            this.bankBranchRepository = bankBranchRepository;
        }

        public List<SalesDefBankBranchInfo> GetBankBranches()
        {
            return GetAll();
        }

        public SalesDefBankBranchInfo GetBankBranch(string id)
        {
            return bankBranchRepository.All().FirstOrDefault(x => x.BankBranchId == id);
        }

        public SalesDefBankBranchInfo SaveBankBranch(SalesDefBankBranchInfo entity)
        {
            if (IsBankBranchExistByCode(entity.BankBranchId))
                Update(entity);
            else
                Add(entity);

            return entity;
        }

        public bool DeleteBankBranch(string id)
        {
            var entity = GetBankBranch(id);
            if (entity != null)
            {
                bankBranchRepository.Delete(entity);
                return true;
            }
            return false;
        }

        public bool IsBankBranchExistByCode(string code)
        {
            return bankBranchRepository.All().Any(x => x.BankBranchId == code);
        }

        public bool IsBankBranchExist(string name)
        {
            return bankBranchRepository.All().Any(x => x.BankBranchName == name);
        }

        public bool IsBankBranchExist(string name, string typeCode)
        {
            return bankBranchRepository.All().Any(x => x.BankBranchName == name && x.BankBranchId != typeCode);
        }

        public IEnumerable<CommonSelectModel> BankBranchSelection()
        {
            return bankBranchRepository.All()
                .Select(x => new CommonSelectModel
                {
                    Code = x.BankBranchId,
                    Name = x.BankBranchName
                });
        }
    }
}
