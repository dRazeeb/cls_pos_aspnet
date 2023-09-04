
using GCTL.Data.Models;
using GCTL.Service.Common;
using GCTL.Service.AccControlLedgers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GCTL.Core.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using GCTL.UI.Core.ViewModels.AccControlLedger;
using GCTL.Core.ViewModels.AccSubControlLedgers;
using GCTL.UI.Core.ViewModels.AccSubControlLedgers;
using GCTL.Service.AccSubControlLedgers;
using GCTL.Service.AccGeneralLedgers;

namespace GCTL.UI.Core.Controllers
{
    public class AccSubControlLedgerController : BaseController
    {
        private readonly IAccSubControlLedgerService SubControlLedgerService;
        private readonly IAccControlLedgerService ControlLedgerService;
        private readonly IAccGeneralLedgerService GeneralLedgerService;
        private readonly ICommonService commonService;        
        private readonly IMapper mapper;
        string strMaxNO = "";
        public AccSubControlLedgerController(IAccSubControlLedgerService SubControlLedgerService,
            IAccControlLedgerService ControlLedgerService,
            IAccGeneralLedgerService GeneralLedgerService,

                              ICommonService commonService,
                              IMapper mapper)
        {
            this.ControlLedgerService = ControlLedgerService;
            this.SubControlLedgerService = SubControlLedgerService;
            this.GeneralLedgerService = GeneralLedgerService;

            this.commonService = commonService;           
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            AccSubControlLedgerPageViewModel model = new AccSubControlLedgerPageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };
            
            ViewBag.CL = new SelectList(ControlLedgerService.ControlLedgerSelection(), "Code", "Name");
            return View(model);
        }


        public IActionResult Setup(string id)
        {
            AccSubControlLedgerSetupViewModel model = new AccSubControlLedgerSetupViewModel();
            //commonService.FindMaxNo(ref strMaxNO, "ControlLedgerCodeNo", "Acc_ControlLedger", 1);
            var result = SubControlLedgerService.GetSubControlLedger(id);
            ViewBag.CL = new SelectList(ControlLedgerService.ControlLedgerSelection(), "Code", "Name");

            if (result != null)
            {
                model = mapper.Map<AccSubControlLedgerSetupViewModel>(result);
                model.Code = id;
                //model.s = (string)result.ControlLedgerCodeNo;
            }
            else
            {
                //model.ControlLedgerCodeNo = strMaxNO;
            }


            return PartialView($"_{nameof(Setup)}", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Setup(AccSubControlLedgerSetupViewModel model)
        {
            if (SubControlLedgerService.IsSubControlLedgerExist(model.ControlLedgerCodeNo,model.SubControlLedgerName, model.SubControlLedgerCodeNo))
            {
                return Json(new { isSuccess = false, message = "Already Exists" });
            }

            if (ModelState.IsValid)
            {
                if (SubControlLedgerService.IsSubControlLedgerExistByCode(model.SubControlLedgerCodeNo))
                {
                    var hasPermission = SubControlLedgerService.UpdatePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        AccSubControlLedger bed = SubControlLedgerService.GetSubControlLedger(model.SubControlLedgerCodeNo) ?? new AccSubControlLedger();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, bed);
                        
                        SubControlLedgerService.SaveSubControlLedger(bed);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = bed.SubControlLedgerCodeNo });
                    }
                    else
                    {

                        return Json(new { isSuccess = false, message = "You have no access" });
                    }

                }
                else
                {
                    var hasPermission = SubControlLedgerService.SavePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        AccSubControlLedger bed = SubControlLedgerService.GetSubControlLedger(model.SubControlLedgerCodeNo) ?? new AccSubControlLedger();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, bed);

                        SubControlLedgerService.SaveSubControlLedger(bed);
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

        public ActionResult Grid(string ControlLedgerCodeNo)
        {
            var resutl = SubControlLedgerService.GetSubControlLedgers(ControlLedgerCodeNo);
            return Json(new { data = resutl });
        }
        public JsonResult MaxID(string ControlLedgerCodeNo)
        {
            commonService.FindAccTwoDigit(ref strMaxNO, "SubControlLedgerCodeNo", "Acc_SubControlLedger", 2, "ControlLedgerCodeNo", ControlLedgerCodeNo);
            string MaxID = ControlLedgerCodeNo + strMaxNO.ToString();
            return Json(MaxID);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var hasPermission = SubControlLedgerService.DeletePermission(LoginInfo.AccessCode);
            if (hasPermission)
            {

                bool success = false;
                var mes = "";
                foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    var entity = GeneralLedgerService.IsGeneralLedgerExistBySCL(item);
                    if (entity)
                    {
                        mes = "Not Deleted. It Exist in sub Control Ledger";
                    }
                    else
                    {
                        success = SubControlLedgerService.DeleteSubControlLedger(item);
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