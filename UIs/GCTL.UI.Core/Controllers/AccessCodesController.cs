using GCTL.Core.Data;
using GCTL.Data.Models;
using GCTL.Service.Common;
using GCTL.Service.Employees;
using GCTL.Service.Users;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GCTL.UI.Core.ViewModels.AccessCodes;
using GCTL.Core.ViewModels.AccessCodes;
using System.Collections.Generic;
using GCTL.Core.DataTables;
using GCTL.UI.Core.Filters;

namespace GCTL.UI.Core.Controllers
{
    public class AccessCodesController : BaseController
    {
        private readonly IAccessCodeService accessCodeService;
        private readonly ICommonService commonService;
        private readonly IEmployeeService employeeService;
        private readonly IEncoderService encoderService;
        private readonly IMapper mapper;
        public AccessCodesController(IAccessCodeService accessCodeService,
                                     ICommonService commonService,
                                     IEmployeeService employeeService,
                                     IEncoderService encoderService,
                                     IMapper mapper)
        {
            this.accessCodeService = accessCodeService;
            this.commonService = commonService;
            this.employeeService = employeeService;
            this.encoderService = encoderService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            AccessCodePageViewModel model = new AccessCodePageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };
            model.Setup = new AccessCodeSetupViewModel
            {
                AccessCodeId = commonService.GenerateNextCode("AccessCodeID", "Core_AccessCode"),
                Accesses = accessCodeService.GetPermissionsAccessCode()
            };

            return View(model);
        }

        public IActionResult Setup(string id)
        {
            AccessCodeSetupViewModel model = new AccessCodeSetupViewModel();
            var result = accessCodeService.GetAccessCode(id);
            if (result != null)
            {
                model = mapper.Map<AccessCodeSetupViewModel>(result);
                model.Code = id;
                model.Accesses = accessCodeService.GetPermissionsAccessCode(id);
            }
            else
            {
                model.AccessCodeId = commonService.GenerateNextCode("AccessCodeID", "Core_AccessCode");
                model.Accesses = accessCodeService.GetMenus();
            }

            return PartialView($"_{nameof(Setup)}", model);
        }

        [HttpPost]
        [RequestFormSizeLimit(valueCountLimit: 3000, Order = 1)]
        [ValidateAntiForgeryToken(Order = 2)]
        public IActionResult Setup(AccessCodeSetupViewModel model)
        {
            var hasPermission = accessCodeService.HasPermission(LoginInfo.AccessCode);
            if (hasPermission)
            {
                accessCodeService.SetPermissions(model);
                return RedirectToAction(nameof(Index));
               // return Json(new { isSuccess = true, message = "Saved Successfully" });
            }
            else
            {
                return RedirectToAction(nameof(Index));
                //return Json(new { isSuccess = false, message = "You have no access" });
            }         
        }

        public ActionResult Grid()
        {
            var result = accessCodeService.GetUniqueAccessCodes();
            return Json(new { data = result });
        }


        [HttpPost]
        public ActionResult Delete(string id)
        {
            bool success = false;
            foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                success = accessCodeService.DeleteAccessCode(item);
            }
            return Json(new { success = success, message = "Deleted Successfully" });
        }

        public IActionResult GetEmployee(string id)
        {
            var result = employeeService.GetEmployeeByCode(id);
            return Json(result);
        }


        public IActionResult Permissions(string id)
        {
            AccessCodeSetupViewModel model = new AccessCodeSetupViewModel();
            var result = accessCodeService.GetAccessCode(id);
            if (result != null)
            {
                model = mapper.Map<AccessCodeSetupViewModel>(result);
                model.Code = id;
                model.Accesses = accessCodeService.GetPermissionsAccessCode(id);
            }
            else
            {
                model.AccessCodeId = commonService.GenerateNextCode("AccessCodeID", "Core_AccessCode");
                model.Accesses = accessCodeService.GetMenus();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Permissions(AccessCodeSetupViewModel model)
        {
            var hasPermission = accessCodeService.HasPermission(LoginInfo.AccessCode);
            if (hasPermission)
            {
                accessCodeService.SetPermissions(model);
                return RedirectToAction(nameof(Index));
                // return Json(new { isSuccess = true, message = "Saved Successfully" });
            }
            else
            {
                return RedirectToAction(nameof(Index));
                //return Json(new { isSuccess = false, message = "You have no access" });
            }
        }

    }
}