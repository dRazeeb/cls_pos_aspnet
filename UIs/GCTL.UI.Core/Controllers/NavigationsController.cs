using GCTL.Core.ViewModels.Navigations;
using GCTL.Data.Models;
using GCTL.Service.Common;
using GCTL.Service.Navigations;
using GCTL.UI.Core.ViewModels.Navigations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GCTL.Core.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCTL.UI.Core.Controllers
{
    public class NavigationsController : BaseController
    {
        private readonly INavigationService navigationService;
        private readonly ICommonService commonService;
        private readonly IMapper mapper;
        public NavigationsController(INavigationService navigationService,
                                     ICommonService commonService,
                                     IMapper mapper)
        {
            this.navigationService = navigationService;
            this.commonService = commonService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            NavigationPageViewModel model = new NavigationPageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };
            model.Setup = new NavigationSetupViewModel
            {
                MenuId = commonService.GenerateNextCode("MenuId", "Core_MenuTab2", 3)
            };

            ViewBag.Parents = new SelectList(navigationService.NavigationSelection(), "Code", "Name", "", "Group");
            return View(model);
        }

        public IActionResult Setup(int id)
        {
            NavigationSetupViewModel model = new NavigationSetupViewModel();
            var result = navigationService.GetNavigation(id);
            if (result != null)
            {
                model = mapper.Map<NavigationSetupViewModel>(result);
                model.Code = result.MenuId;
                model.AutoId = result.AutoId;
            }
            else
            {
                model.MenuId = commonService.GenerateNextCode("MenuId", "Core_MenuTab2", 3);
            }

            ViewBag.Parents = new SelectList(navigationService.NavigationSelection(), "Code", "Name", model.ParentId, "Group");
            return PartialView($"_{nameof(Setup)}", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Setup(NavigationSetupViewModel model)
        {
            //if (navigationService.IsNavigationExist(model.Title, model.MenuId))
            //{
            //    return Json(new { isSuccess = false, message = "Already Exists" });
            //}

            if (ModelState.IsValid)
            {
                CoreMenuTab2 navigation = navigationService.GetNavigation(model.AutoId) ?? new CoreMenuTab2();
                model.ToAudit(LoginInfo, model.AutoId > 0);
                mapper.Map(model, navigation);

                if (model.AutoId == 0)
                    navigation.OrderBy = model.OrderBy > 0 ? model.OrderBy : navigationService.LastOrder() + 1;

                navigationService.SaveNavigation(navigation);
                return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = navigation.MenuId });
            }

            return Json(new { success = false, message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
        }

        public ActionResult Grid()
        {
            var result = navigationService.GetNavigations();
            return PartialView("_Grid", result);
            // return Json(new { data = result });
        }


        [HttpPost]
        public ActionResult Delete(string id)
        {
            bool success = false;
            foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                int navigationId = 0;
                int.TryParse(item, out navigationId);
                success = navigationService.DeleteNavigation(navigationId);
            }

            return Json(new { success = success, message = "Deleted Successfully" });
        }


        [HttpPost]
        public JsonResult CheckAvailability(string name, string code)
        {
            if (navigationService.IsNavigationExist(name, code))
            {
                return Json(new { isSuccess = true, message = "Already Exists" });
            }

            return Json(new { isSuccess = false });
        }

        [HttpPost]
        public IActionResult Orders(int[] items)
        {
            bool response = false;
            try
            {
                response = navigationService.Order(items);
            }
            catch (Exception)
            {
                //throw ex;
            }

            return RedirectToAction(nameof(Index));
        }

    }
}