using GCTL.Core.ViewModels.AccSubsidiaryLedgers;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;

namespace GCTL.Service.AccSubsidiaryLedgers
{
    public interface IAccSubsidiaryLedgerService
    {
        List<AccSubsidiaryLedgerSetupViewModel> GetInfoList(string ControlLedgerCodeNo, string SubControlLedgerCodeNo,string GeneralLedgerCodeNo);
        AccSubsidiaryLedger GetInfo(string code);
        AccSubsidiaryLedgerSetupViewModel GetInfoForView(string code);

        bool Delete(string id);
        AccSubsidiaryLedger Save(AccSubsidiaryLedger entity);
        bool IsExistByCode(string code);
        bool IsExistByGL(string code);
        bool IsExist(string GeneralLedgerCodeNo, string name);
        bool IsExist(string GeneralLedgerCodeNo, string name, string typeCode);
        IEnumerable<CommonSelectModel> DropSelection();
        IEnumerable<CommonSelectModel> GetInfoBYParenet(string GeneralLedgerCodeNo);

        bool SavePermission(string accessCode);
        bool UpdatePermission(string accessCode);
        bool DeletePermission(string accessCode);
    }
}
