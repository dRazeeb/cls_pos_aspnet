using GCTL.Core.Data;
using GCTL.Core.ViewModels.Common;
using GCTL.Core.ViewModels.Navigations;
using GCTL.Data.Models;
using GCTL.Service.Users;

namespace GCTL.Service.Navigations
{
    public class NavigationService : AppService<CoreMenuTab2>, INavigationService
    {
        private readonly IRepository<CoreMenuTab2> navigationRepository;
        private readonly IAccessCodeService accessCodeService;

        public NavigationService(IRepository<CoreMenuTab2> navigationRepository,
                                 IAccessCodeService accessCodeService)
            : base(navigationRepository)
        {
            this.navigationRepository = navigationRepository;
            this.accessCodeService = accessCodeService;
        }

        public List<NavigationModel> GetNavigations()
        {
            List<NavigationModel> navigations = new List<NavigationModel>();
            var navs = navigationRepository.All()
                .Select(x => new NavigationModel
                {
                    Id = x.AutoId,
                    MenuId = x.MenuId,
                    Title = x.Title,
                    ParentId = x.ParentId,
                    Parent = navigationRepository.All().FirstOrDefault(n => n.MenuId == x.ParentId).Title,
                    ControllerName = x.ControllerName,
                    ViewName = x.ViewName,
                    Icon = x.Icon,
                    OrderBy = x.OrderBy,
                    IsActive = x.IsActive
                })
                .OrderBy(x => x.ParentId)
                .ThenBy(x => x.OrderBy)
                .ToList();

            foreach (var parent in navs.Where(x => x.ParentId == "0"))
            {
                parent.Navigations = navs.Where(x => x.ParentId != "0" && x.ParentId == parent.MenuId).ToList();
                navigations.Add(parent);
            }

            return navigations;
        }

        public CoreMenuTab2 GetNavigationByMenuId(string id)
        {
            return navigationRepository.All().FirstOrDefault(x => x.MenuId == id);
        }

        public CoreMenuTab2 GetNavigation(object id)
        {
            return navigationRepository.GetById(id);
        }
        public CoreMenuTab2 SaveNavigation(CoreMenuTab2 entity)
        {
            if (IsNavigationExistByCode(entity.MenuId))
            {
                Update(entity);
                accessCodeService.UpdateAccessCode(entity);
            }
            else
                Add(entity);

            return entity;
        }

        public bool DeleteNavigation(int id)
        {
            var entity = GetNavigation(id);
            if (entity != null)
            {
                navigationRepository.Delete(entity);
                return true;
            }
            return false;
        }

        public bool IsNavigationExistByCode(string code)
        {
            return navigationRepository.All().Any(x => x.MenuId == code);
        }

        public bool IsNavigationExist(string name)
        {
            return navigationRepository.All().Any(x => x.Title == name);
        }

        public bool IsNavigationExist(string name, string typeCode)
        {
            return navigationRepository.All().Any(x => x.Title == name && x.MenuId != typeCode);
        }

        public IEnumerable<CommonSelectModel> NavigationSelection()
        {
            return navigationRepository.All().Where(x => x.ControllerName == null).Select(x => new CommonSelectModel
            {
                Code = x.MenuId,
                Name = x.Title,
                Group = navigationRepository.All().FirstOrDefault(n => n.MenuId == x.ParentId).Title ?? "Modules"
            }).ToList();
        }

        public bool Order(int[] items)
        {
            int order = 1;
            foreach (var item in items)
            {
                var data = navigationRepository.All().FirstOrDefault(x => x.AutoId == item);
                if (data != null)
                {
                    data.OrderBy = order;
                    SaveNavigation(data);
                    order++;
                }
            }

            return true;
        }

        public int LastOrder()
        {
            if (navigationRepository.All().Any())
                return navigationRepository.All().Select(x => x.OrderBy).Max();

            return 1;
        }
    }
}
