using GCTL.Core.ViewModels.Common;
using GCTL.Data.Models;


namespace GCTL.Service.AccVoucherTypes
{
    public interface IAccVoucherTypeService
    {
        List<AccVoucherType> GetInfoList();
        AccVoucherType GetInfo(string code);
        bool Delete(string id);
        AccSubSubsidiaryLedger Save(AccSubSubsidiaryLedger entity);
        bool IsExistByCode(string code);
        bool IsExist( string name);
        bool IsExist( string name, string typeCode);
        IEnumerable<CommonSelectModel> DropSelection();
        bool SavePermission(string accessCode);
        bool UpdatePermission(string accessCode);
        bool DeletePermission(string accessCode);
    }
}
