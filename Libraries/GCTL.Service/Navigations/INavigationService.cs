using GCTL.Core.ViewModels.Common;
using GCTL.Core.ViewModels.Navigations;
using GCTL.Data.Models;

namespace GCTL.Service.Navigations
{
    public interface INavigationService
    {
        List<NavigationModel> GetNavigations();
        CoreMenuTab2 GetNavigation(object id);
        CoreMenuTab2 GetNavigationByMenuId(string code);
        bool DeleteNavigation(int id);
        CoreMenuTab2 SaveNavigation(CoreMenuTab2 entity);
        bool IsNavigationExistByCode(string code);
        bool IsNavigationExist(string name);
        bool IsNavigationExist(string name, string typeCode);
        IEnumerable<CommonSelectModel> NavigationSelection();
        bool Order(int[] items);
        int LastOrder();
    }
}