
using GCTL.Data.Models;
using GCTL.Service.Common;
using GCTL.Service.AccControlLedgers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GCTL.Core.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using GCTL.UI.Core.ViewModels.AccControlLedger;
using GCTL.Core.ViewModels.AccControlLedger;
using GCTL.Service.AccSubControlLedgers;

namespace GCTL.UI.Core.Controllers
{
    public class AccControlLedgerController : BaseController
    {
        private readonly IAccControlLedgerService ControlLedgerService;
        private readonly IAccSubControlLedgerService SubControlLedgerService;

        private readonly ICommonService commonService;
     
        private readonly IMapper mapper;
        string strMaxNO = "";
        public AccControlLedgerController(IAccControlLedgerService ControlLedgerService,
            IAccSubControlLedgerService SubControlLedgerService,
                              ICommonService commonService,
                              IMapper mapper)
        {
            this.ControlLedgerService = ControlLedgerService;
            this.SubControlLedgerService = SubControlLedgerService;

            this.commonService = commonService;           
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            AccControlLedgerPageViewModel model = new AccControlLedgerPageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };
            commonService.FindMaxNo(ref strMaxNO, "ControlLedgerCodeNo", "Acc_ControlLedger", 1);
            model.Setup = new AccControlLedgerSetupViewModel
            {
                ControlLedgerCodeNo = strMaxNO
            };

            return View(model);
        }


        public IActionResult Setup(string id)
        {
            AccControlLedgerSetupViewModel model = new AccControlLedgerSetupViewModel();
            commonService.FindMaxNo(ref strMaxNO, "ControlLedgerCodeNo", "Acc_ControlLedger", 1);
            var result = ControlLedgerService.GetControlLedger(id);
            if (result != null)
            {
                model = mapper.Map<AccControlLedgerSetupViewModel>(result);
                model.Code = id;
                model.ControlLedgerCodeNo = (string)result.ControlLedgerCodeNo;
            }
            else
            {
                model.ControlLedgerCodeNo = strMaxNO;
            }


            return PartialView($"_{nameof(Setup)}", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Setup(AccControlLedgerSetupViewModel model)
        {
            if (ControlLedgerService.IsControlLedgerExist(model.ControlLedgerName, model.ControlLedgerCodeNo))
            {
                return Json(new { isSuccess = false, message = "Already Exists" });
            }

            if (ModelState.IsValid)
            {
                if (ControlLedgerService.IsControlLedgerExistByCode(model.ControlLedgerCodeNo))
                {
                    var hasPermission = ControlLedgerService.UpdatePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        AccControlLedger bed = ControlLedgerService.GetControlLedger(model.ControlLedgerCodeNo) ?? new AccControlLedger();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, bed);
                        
                        ControlLedgerService.SaveControlLedger(bed);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = bed.ControlLedgerCodeNo });
                    }
                    else
                    {

                        return Json(new { isSuccess = false, message = "You have no access" });
                    }

                }
                else
                {
                    var hasPermission = ControlLedgerService.SavePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        AccControlLedger bed = ControlLedgerService.GetControlLedger(model.ControlLedgerCodeNo) ?? new AccControlLedger();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, bed);
                        
                        ControlLedgerService.SaveControlLedger(bed);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = bed.ControlLedgerCodeNo });
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
            var resutl = ControlLedgerService.GetControlLedgers();
            return Json(new { data = resutl });
        }


        [HttpPost]
        public ActionResult Delete(string id)
        {
            var hasPermission = ControlLedgerService.DeletePermission(LoginInfo.AccessCode);
            if (hasPermission)
            {
                bool success = false;
                var mes = "";
                foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    var entity = SubControlLedgerService.IsSubControlLedgerExistByCL(item);
                    if(entity)
                    {
                        mes = "Not Deleted. It Exist in sub Control Ledger";
                    }
                    else
                    {
                        success = ControlLedgerService.DeleteControlLedger(item);
                        mes = "Deleted Successfully";
                    }                    
                }
                return Json(new { success = success, message = mes });
            }
            else
            {
                return Json(new { isSuccess = false, message = "You have no access" });
            }

           
        }


        //[HttpPost]
        //public JsonResult CheckAvailability(string name, string code)
        //{
        //    if (bedService.IsBedExist(name, code))
        //    {
        //        return Json(new { isSuccess = true, message = "Already Exists" });
        //    }

        //    return Json(new { isSuccess = false });
        //}
    }
}