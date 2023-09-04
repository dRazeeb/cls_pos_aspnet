using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;

namespace GCTL.Service.Banks
{
    public interface IBankService
    {
        List<SalesDefBankInfo> GetBanks();
        SalesDefBankInfo GetBank(string code); 
        bool DeleteBank(string id);    
        SalesDefBankInfo SaveBank(SalesDefBankInfo entity);
        bool IsBankExistByCode(string code);
        bool IsBankExist(string name);
        bool IsBankExist(string name, string typeCode);
        IEnumerable<CommonSelectModel> BankSelection();
    }
}