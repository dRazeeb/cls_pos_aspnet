using GCTL.Core.Data;
using GCTL.Core.ViewModels.BankAccounts;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;

namespace GCTL.Service.BankAccounts
{
    public class BankAccountService : AppService<CoreBankAccountInformation>, IBankAccountService
    {
        private readonly IRepository<CoreBankAccountInformation> bankAccountRepository;
        private readonly IRepository<SalesDefBankInfo> bankRepository;
        private readonly IRepository<SalesDefBankBranchInfo> bankBranchRepository;

        public BankAccountService(IRepository<CoreBankAccountInformation> bankAccountRepository,
                                  IRepository<SalesDefBankInfo> bankRepository,
                                  IRepository<SalesDefBankBranchInfo> bankBranchRepository)
            : base(bankAccountRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
            this.bankRepository = bankRepository;
            this.bankBranchRepository = bankBranchRepository;
        }

        public List<BankAccountModel> GetBankAccounts()
        {
            return (from bankAccount in bankAccountRepository.All()
                    join bank in bankRepository.All()        
                    on bankAccount.BankId equals bank.BankId into accountBank
                    from bank in accountBank.DefaultIfEmpty()
                    join branch in bankBranchRepository.All()
                    on bankAccount.BranchId equals branch.BankBranchId into accountBankBranch
                    from branch in accountBankBranch.DefaultIfEmpty()
                    select new BankAccountModel
                    {
                        AutoId = bankAccount.AutoId,
                        AccInfoId = bankAccount.AccInfoId,
                        AccountName = bankAccount.AccountName,
                        AccountNo = bankAccount.AccountNo,
                        BankName = bank.BankName,
                        BranchName = branch.BankBranchName
                    }).ToList();
        }

        public CoreBankAccountInformation GetBankAccount(string id)
        {
            return bankAccountRepository.All().FirstOrDefault(x => x.AccInfoId == id);
        }

        public CoreBankAccountInformation SaveBankAccount(CoreBankAccountInformation entity)
        {
            if (IsBankAccountExistByCode(entity.AccInfoId))
                Update(entity);
            else
                Add(entity);

            return entity;
        }

        public bool DeleteBankAccount(string id)
        {
            var entity = GetBankAccount(id);
            if (entity != null)
            {
                bankAccountRepository.Delete(entity);
                return true;
            }
            return false;
        }

        public bool IsBankAccountExistByCode(string code)
        {
            return bankAccountRepository.All().Any(x => x.AccInfoId == code);
        }

        public bool IsBankAccountExist(string name)
        {
            return bankAccountRepository.All().Any(x => x.AccountName == name);
        }

        public bool IsBankAccountExist(string name, string typeCode)
        {
            return bankAccountRepository.All().Any(x => x.AccountName == name && x.AccInfoId != typeCode);
        }

        public IEnumerable<CommonSelectModel> BankAccountSelection()
        {
            return bankAccountRepository.All()
                .Select(x => new CommonSelectModel
                {
                    Code = x.AccountNo,
                    Name = x.AccountName
                });
        }

        public IEnumerable<CommonSelectModel> BankAccountDetailsSelection()
        {
            return (from bankAccount in bankAccountRepository.All()
                    join bank in bankRepository.All()
                    on bankAccount.BankId equals bank.BankId
                    //join branch in bankBranchRepository.All()
                    //on bankAccount.BranchId equals branch.BankBranchId
                    select new CommonSelectModel
                    {
                        Code = bankAccount.AccountNo,
                        Name = $"{bank.BankName} - {bankAccount.AccountName} - {bankAccount.AccountNo}"
                    });
        }

    }
}
