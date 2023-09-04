using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Service.AccControlLedgers
{
    public interface IAccControlLedgerService
    {
        List<AccControlLedger> GetControlLedgers();
        AccControlLedger GetControlLedger(string code);
        bool DeleteControlLedger(string id);
        AccControlLedger SaveControlLedger(AccControlLedger entity);
        bool IsControlLedgerExistByCode(string code);
        bool IsControlLedgerExist(string name);
        bool IsControlLedgerExist(string name, string typeCode);
        IEnumerable<CommonSelectModel> ControlLedgerSelection();

        bool SavePermission(string accessCode);
        bool UpdatePermission(string accessCode);
        bool DeletePermission(string accessCode);
    }
}
