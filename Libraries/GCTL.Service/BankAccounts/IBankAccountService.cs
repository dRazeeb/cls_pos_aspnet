using GCTL.Core.ViewModels.BankAccounts;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;

namespace GCTL.Service.BankAccounts
{
    public interface IBankAccountService
    {
        List<BankAccountModel> GetBankAccounts();
        CoreBankAccountInformation GetBankAccount(string code); 
        bool DeleteBankAccount(string id);    
        CoreBankAccountInformation SaveBankAccount(CoreBankAccountInformation entity);
        bool IsBankAccountExistByCode(string code);
        bool IsBankAccountExist(string name);
        bool IsBankAccountExist(string name, string typeCode);
        IEnumerable<CommonSelectModel> BankAccountSelection();
        IEnumerable<CommonSelectModel> BankAccountDetailsSelection();

    }
}