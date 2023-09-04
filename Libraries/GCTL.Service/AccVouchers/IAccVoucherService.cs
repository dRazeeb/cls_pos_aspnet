using GCTL.Core.ViewModels.AccVouchers;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;
using GCTL.Core.ViewModels.Accounts;

namespace GCTL.Service.AccVouchers
{
    public interface IAccVoucherService
    {
        List<AccVoucherSetupViewModel> GetInfoList(string FromDate, string ToDate);

        AccVoucherSetupViewModel GetInfo(string code);
        List<AccVoucherDetailsSetupViewModel> GetInfoDetails(string code);
        List<VoucherEntryReportModel> GetInfoForReport(string code);
        bool Delete(string id);
        bool DeleteByInvoiceNo(string id);

        bool IsASSLexistInVE(string code);
        bool Save(AccVoucherSetupViewModel entity, UserInfoViewModel loginInfo);
        bool Update(AccVoucherSetupViewModel entity, UserInfoViewModel loginInfo);
        bool IsExistByCode(string code);       
        bool SavePermission(string accessCode);
        bool UpdatePermission(string accessCode);
        bool DeletePermission(string accessCode);
    }
}
