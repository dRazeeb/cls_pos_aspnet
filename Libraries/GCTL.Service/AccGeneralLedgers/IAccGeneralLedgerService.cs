using GCTL.Core.ViewModels.AccGeneralLedgers;
using GCTL.Core.ViewModels.AccSubControlLedgers;
using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Service.AccGeneralLedgers
{
    public interface IAccGeneralLedgerService
    {
        List<AccGeneralLedgerSetupViewModel> GetGeneralLedgers(string ControlLedgerCodeNo,string SubControlLedgerCodeNo);
        AccGeneralLedger GetGeneralLedger(string code);
        AccGeneralLedgerSetupViewModel GetGeneralLedgerForView(string code);

        bool DeleteGeneralLedger(string id);
        AccGeneralLedger SaveGeneralLedger(AccGeneralLedger entity);
        bool IsGeneralLedgerExistByCode(string code);
        bool IsGeneralLedgerExistBySCL(string code);
        bool IsGeneralLedgerExist(string SubControlLedgerCodeNo, string name);
        bool IsGeneralLedgerExist(string SubControlLedgerCodeNo, string name, string typeCode);
        IEnumerable<CommonSelectModel> GeneralLedgerSelection();
        IEnumerable<CommonSelectModel> GetInfoBYParent(string SubControlLedgerCodeNo);
        bool SavePermission(string accessCode);
        bool UpdatePermission(string accessCode);
        bool DeletePermission(string accessCode);
    }
}
