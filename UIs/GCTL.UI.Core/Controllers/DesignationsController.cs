using GCTL.Core.ViewModels.Designations;
using GCTL.Data.Models;
using GCTL.Service.Common;
using GCTL.Service.Designations;
using GCTL.UI.Core.ViewModels.Designations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GCTL.Core.Helpers;

namespace GCTL.UI.Core.Controllers
{
    public class DesignationsController : BaseController
    {
        private readonly IDesignationService designationService;
        private readonly ICommonService commonService;
        private readonly IMapper mapper;
        string strMaxNO = "";
        public DesignationsController(IDesignationService designationService,
                                      ICommonService commonService,
                                      IMapper mapper)
        {
            this.designationService = designationService;
            this.commonService = commonService;
            this.mapper = mapper;
        }

        public IActionResult Index(bool child = false)
        {
            DesignationPageViewModel model = new DesignationPageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };
            commonService.FindMaxNo(ref strMaxNO, "DesignationCode", "HRM_Def_Designation", 3);
            model.Setup = new DesignationSetupViewModel
            {
                DesignationCode = strMaxNO
            };

            if (child)
                return PartialView(model);

            return View(model);
        }


        public IActionResult Setup(string id)
        {
            DesignationSetupViewModel model = new DesignationSetupViewModel();
            commonService.FindMaxNo(ref strMaxNO, "DesignationCode", "HRM_Def_Designation", 3);
            var result = designationService.GetDesignation(id);
            if (result != null)
            {
                model = mapper.Map<DesignationSetupViewModel>(result);
                model.Code = id;
                model.Id = (int)result.AutoId;
            }
            else
            {
                model.DesignationCode = strMaxNO;
            }

            return PartialView($"_{nameof(Setup)}", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Setup(DesignationSetupViewModel model)
        {
            if (designationService.IsDesignationExist(model.DesignationName, model.DesignationCode))
            {
                return Json(new { isSuccess = false, message = "Already Exists" });
            }

            if (ModelState.IsValid)
            {
                if (designationService.IsDesignationExistByCode(model.DesignationCode))
                {
                    var hasPermission = designationService.UpdatePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        HrmDefDesignation designation = designationService.GetDesignation(model.DesignationCode) ?? new HrmDefDesignation();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, designation);
                        designationService.SaveDesignation(designation);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = designation.DesignationCode });
                    }
                    else
                    {

                        return Json(new { isSuccess = false, message = "You have no access" });
                    }

                }
                else
                {
                    var hasPermission = designationService.SavePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        HrmDefDesignation designation = designationService.GetDesignation(model.DesignationCode) ?? new HrmDefDesignation();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, designation);
                        designationService.SaveDesignation(designation);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = designation.DesignationCode });
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
            var resutl = designationService.GetDesignations();
            return Json(new { data = resutl });
        }


        [HttpPost]
        public ActionResult Delete(string id)
        {
            var hasPermission = designationService.DeletePermission(LoginInfo.AccessCode);
            if (hasPermission)
            {
                bool success = false;
                foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    success = designationService.DeleteDesignation(item);
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
            if (designationService.IsDesignationExist(name, code))
            {
                return Json(new { isSuccess = true, message = "Already Exists" });
            }

            return Json(new { isSuccess = false });
        }
    }
}