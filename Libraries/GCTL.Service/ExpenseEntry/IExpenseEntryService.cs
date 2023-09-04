using GCTL.Core.ViewModels.Accounts;
using GCTL.Core.ViewModels.ExpenseEntry;
using GCTL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Service.ExpenseEntry
{
    public interface IExpenseEntryService
    {
        List<ExpenseEntrySetupViewModel> GetInfoList(string FromDate,string ToDate);
        ExpenseEntrySetupViewModel GetInfo(string code);    
        bool Delete(string id);
        bool Save(ExpenseEntrySetupViewModel entity, UserInfoViewModel loginInfo);
        bool Update(ExpenseEntrySetupViewModel entity, UserInfoViewModel loginInfo);
        bool IsExistByCode(string code);
        bool ExcuteInVoucherEntryPV(string ExpenseCode);
        bool SavePermission(string accessCode);
        bool UpdatePermission(string accessCode);
        bool DeletePermission(string accessCode);
    }
}
