using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;

namespace GCTL.Service.BankBranches
{
    public interface IBankBranchService
    {
        List<SalesDefBankBranchInfo> GetBankBranches();
        SalesDefBankBranchInfo GetBankBranch(string code); 
        bool DeleteBankBranch(string id);    
        SalesDefBankBranchInfo SaveBankBranch(SalesDefBankBranchInfo entity);
        bool IsBankBranchExistByCode(string code);
        bool IsBankBranchExist(string name);
        bool IsBankBranchExist(string name, string typeCode);
        IEnumerable<CommonSelectModel> BankBranchSelection();
    }
}