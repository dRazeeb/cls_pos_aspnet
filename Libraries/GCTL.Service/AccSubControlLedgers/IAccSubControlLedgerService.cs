using GCTL.Core.ViewModels.AccSubControlLedgers;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;


namespace GCTL.Service.AccSubControlLedgers
{
    public  interface IAccSubControlLedgerService
    {
        List<AccSubControlLedgerSetupViewModel> GetSubControlLedgers(string ControlLedgerCodeNo);
        AccSubControlLedger GetSubControlLedger(string code);
        bool DeleteSubControlLedger(string id);
        AccSubControlLedger SaveSubControlLedger(AccSubControlLedger entity);
        bool IsSubControlLedgerExistByCode(string code);
        bool IsSubControlLedgerExistByCL(string code);
        bool IsSubControlLedgerExist(string ControlLedgerCode,string name);
        bool IsSubControlLedgerExist(string ControlLedgerCode, string name, string typeCode);
        IEnumerable<CommonSelectModel> SubControlLedgerSelection();
        IEnumerable<CommonSelectModel> SubControlLedgerSelectionBYCl(string ControlLedgerCodeNo);

        bool SavePermission(string accessCode);
        bool UpdatePermission(string accessCode);
        bool DeletePermission(string accessCode);
    }
}
