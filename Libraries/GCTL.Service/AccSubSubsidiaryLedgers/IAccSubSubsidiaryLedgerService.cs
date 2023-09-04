using GCTL.Core.ViewModels.AccSubSubsidiaryLedgers;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;


namespace GCTL.Service.AccSubSubsidiaryLedgers
{
    public interface IAccSubSubsidiaryLedgerService
    {
        List<AccSubSubsidiaryLedgerSetupViewModel> GetInfoList(string ControlLedgerCodeNo,
            string SubControlLedgerCodeNo, string GeneralLedgerCodeNo, string SubsidiaryLedgerCodeNo);

        List<AccSubSubsidiaryLedgerSetupViewModel> GetAccHeadGrid(string SubSusidiaryLedgerCodeNo);
        AccSubSubsidiaryLedger GetInfo(string code);
        AccSubSubsidiaryLedgerSetupViewModel GetInfoForView(string code);

        bool Delete(string id);
        AccSubSubsidiaryLedger Save(AccSubSubsidiaryLedger entity);
        bool IsExistByCode(string code);
        bool IsExistBySL(string code);
        bool IsExist(string SubsidiaryLedgerCodeNo, string name);
        bool IsExist(string SubsidiaryLedgerCodeNo, string name, string typeCode);
        IEnumerable<CommonSelectModel> DropSelection();
        IEnumerable<CommonSelectModel> getBkashkDropSelection();
        IEnumerable<CommonSelectModel> getExpenseDropSelection();
        IEnumerable<CommonSelectModel> GetInfoBYParent(string SubsidiaryLedgerCodeNo);

        bool SavePermission(string accessCode);
        bool UpdatePermission(string accessCode);
        bool DeletePermission(string accessCode);
    }
}
