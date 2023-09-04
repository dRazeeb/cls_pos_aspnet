using GCTL.Core.ViewModels.Departments;
using GCTL.Data.Models;
using GCTL.Service.Common;
using GCTL.Service.Departments;
using GCTL.UI.Core.ViewModels.Departments;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GCTL.Core.Helpers;

namespace GCTL.UI.Core.Controllers
{
    public class DepartmentsController : BaseController
    {
        private readonly IDepartmentService departmentService;
        private readonly ICommonService commonService;
        private readonly IMapper mapper;
        string strMaxNO = "";
        public DepartmentsController(IDepartmentService departmentService,
                                     ICommonService commonService,
                                     IMapper mapper)
        {
            this.departmentService = departmentService;
            this.commonService = commonService;
            this.mapper = mapper;
        }

        public IActionResult Index(bool child = false)
        {
            DepartmentPageViewModel model = new DepartmentPageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };
            commonService.FindMaxNo(ref strMaxNO, "DepartmentCode", "HRM_Def_Department", 2);
            model.Setup = new DepartmentSetupViewModel
            {
                DepartmentCode = strMaxNO
            };

            if (child)
                return PartialView(model);

            return View(model);
        }


        public IActionResult Setup(string id)
        {
            DepartmentSetupViewModel model = new DepartmentSetupViewModel();
            commonService.FindMaxNo(ref strMaxNO, "DepartmentCode", "HRM_Def_Department", 2);
            var result = departmentService.GetDepartment(id);
            if (result != null)
            {
                model = mapper.Map<DepartmentSetupViewModel>(result);
                model.Code = id;
                model.Id = (int)result.AutoId;
            }
            else
            {
                model.DepartmentCode = strMaxNO;
            }

            return PartialView($"_{nameof(Setup)}", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Setup(DepartmentSetupViewModel model)
        {
            if (departmentService.IsDepartmentExist(model.DepartmentName, model.DepartmentCode))
            {
                return Json(new { isSuccess = false, message = "Already Exists" });
            }

            if (ModelState.IsValid)
            {
                if (departmentService.IsDepartmentExistByCode(model.DepartmentCode))
                {
                    var hasPermission = departmentService.UpdatePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        HrmDefDepartment department = departmentService.GetDepartment(model.DepartmentCode) ?? new HrmDefDepartment();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, department);
                        departmentService.SaveDepartment(department);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = department.DepartmentCode });
                    }
                    else
                    {

                        return Json(new { isSuccess = false, message = "You have no access" });
                    }

                }
                else
                {
                    var hasPermission = departmentService.SavePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        HrmDefDepartment department = departmentService.GetDepartment(model.DepartmentCode) ?? new HrmDefDepartment();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, department);
                        departmentService.SaveDepartment(department);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = department.DepartmentCode });
                    }
                    else
                    {

                        return Json(new { isSuccess = false, message = "You have no access" });
                    }

                }

               
            }

            return Json(new { success = false, message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
        }

        public ActionResult Grid()
        {
            var result = departmentService.GetDepartments();
            return Json(new { data = result });
        }


        [HttpPost]
        public ActionResult Delete(string id)
        {
            var hasPermission = departmentService.DeletePermission(LoginInfo.AccessCode);
            if (hasPermission)
            {
                bool success = false;
                foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    success = departmentService.DeleteDepartment(item);
                }

                return Json(new { success = success, message = "Deleted Successfully" });
            }
            else
            {
                return Json(new { isSuccess = false, message = "You have no access" });
            }
           
        }


        [HttpPost]
        public JsonResult CheckAvailability(string name, string code)
        {
            if (departmentService.IsDepartmentExist(name, code))
            {
                return Json(new { isSuccess = true, message = "Already Exists" });
            }

            return Json(new { isSuccess = false });
        }
    }
}