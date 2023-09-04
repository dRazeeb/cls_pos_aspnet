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
using GCTL.Service.AccSubsidiaryLedgers;
using GCTL.Service.AccSubSubsidiaryLedgers;
using GCTL.UI.Core.ViewModels.AccSubsidiaryLedgers;
using GCTL.Core.ViewModels.AccGeneralLedgers;
using GCTL.Core.ViewModels.AccSubsidiaryLedgers;

namespace GCTL.UI.Core.Controllers
{
    public class AccSubsidiaryLedgerController : BaseController
    {
        private readonly IAccSubSubsidiaryLedgerService SubSubsidiaryLedgerService;

        private readonly IAccSubsidiaryLedgerService SubsidiaryLedgerService;
        private readonly IAccGeneralLedgerService GeneralLedgerService;
        private readonly IAccSubControlLedgerService SubControlLedgerService;
        private readonly IAccControlLedgerService ControlLedgerService;

        private readonly ICommonService commonService;        
        private readonly IMapper mapper;
        string strMaxNO = "";
        public AccSubsidiaryLedgerController(IAccSubSubsidiaryLedgerService SubSubsidiaryLedgerService,
            IAccSubsidiaryLedgerService SubsidiaryLedgerService, 
            IAccGeneralLedgerService GeneralLedgerService,
            IAccSubControlLedgerService SubControlLedgerService,
            IAccControlLedgerService ControlLedgerService,
                              ICommonService commonService,
                              IMapper mapper)
        {
            this.SubSubsidiaryLedgerService = SubSubsidiaryLedgerService;
            this.SubsidiaryLedgerService = SubsidiaryLedgerService;
            this.GeneralLedgerService = GeneralLedgerService;
            this.ControlLedgerService = ControlLedgerService;
            this.SubControlLedgerService = SubControlLedgerService;
            this.commonService = commonService;           
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            AccSubsidiaryLedgerPageViewModel model = new AccSubsidiaryLedgerPageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };
            
            ViewBag.CL = new SelectList(ControlLedgerService.ControlLedgerSelection(), "Code", "Name");
            ViewBag.SCL = new SelectList(SubControlLedgerService.SubControlLedgerSelection(), "Code", "Name");
            ViewBag.GL = new SelectList(GeneralLedgerService.GeneralLedgerSelection(), "Code", "Name");

            return View(model);
        }


        public IActionResult Setup(string id)
        {
            AccSubsidiaryLedgerSetupViewModel model = new AccSubsidiaryLedgerSetupViewModel();
            var result = SubsidiaryLedgerService.GetInfoForView(id);
            ViewBag.CL = new SelectList(ControlLedgerService.ControlLedgerSelection(), "Code", "Name");
            ViewBag.SCL = new SelectList(SubControlLedgerService.SubControlLedgerSelection(), "Code", "Name");
            ViewBag.GL = new SelectList(GeneralLedgerService.GeneralLedgerSelection(), "Code", "Name");
            if (result != null)
            {
                model = mapper.Map<AccSubsidiaryLedgerSetupViewModel>(result);
                model.Code = id;
            }
            else
            {
                //model.ControlLedgerCodeNo = strMaxNO;
            }


            return PartialView($"_{nameof(Setup)}", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Setup(AccSubsidiaryLedgerSetupViewModel model)
        {
            if (SubsidiaryLedgerService.IsExist(model.GeneralLedgerCodeNo,model.SubsidiaryLedgerName, model.SusidiaryLedgerCodeNo))
            {
                return Json(new { isSuccess = false, message = "Already Exists" });
            }

            if (ModelState.IsValid)
            {
                if (SubsidiaryLedgerService.IsExistByCode(model.SusidiaryLedgerCodeNo))
                {
                    var hasPermission = SubsidiaryLedgerService.UpdatePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        AccSubsidiaryLedger bed = SubsidiaryLedgerService.GetInfo(model.SusidiaryLedgerCodeNo) ?? new AccSubsidiaryLedger();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, bed);
                        SubsidiaryLedgerService.Save(bed);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = bed.SusidiaryLedgerCodeNo });
                    }
                    else
                    {

                        return Json(new { isSuccess = false, message = "You have no access" });
                    }

                }
                else
                {
                    var hasPermission = SubsidiaryLedgerService.SavePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        AccSubsidiaryLedger bed = SubsidiaryLedgerService.GetInfo(model.SusidiaryLedgerCodeNo) ?? new AccSubsidiaryLedger();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, bed);
                        SubsidiaryLedgerService.Save(bed);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = bed.SusidiaryLedgerCodeNo });
                    }
                    else
                    {

                        return Json(new { isSuccess = false, message = "You have no access" });
                    }
                }   
            }
            return Json(new { success = false, message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
        }

        public ActionResult Grid(string ControlLedgerCodeNo,string SubControlLedgerCodeNo,string GeneralLedgerCodeNo)
        {
            var resutl = SubsidiaryLedgerService.GetInfoList(ControlLedgerCodeNo,SubControlLedgerCodeNo, GeneralLedgerCodeNo);
            return Json(new { data = resutl });
        }
        public JsonResult MaxID(string GeneralLedgerCodeNo)
        {
            commonService.FindAccFourDigit(ref strMaxNO, "SusidiaryLedgerCodeNo", "Acc_SubsidiaryLedger", 4, "GeneralLedgerCodeNo", GeneralLedgerCodeNo);
            string MaxID = GeneralLedgerCodeNo + strMaxNO.ToString();
            return Json(MaxID);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var hasPermission = SubsidiaryLedgerService.DeletePermission(LoginInfo.AccessCode);
            if (hasPermission)
            {

                bool success = false;
                var mes = "";
                foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    var entity = SubSubsidiaryLedgerService.IsExistBySL(item);
                    if (entity)
                    {
                        mes = "Not Deleted. It Exist in sub Control Ledger";
                    }
                    else
                    {
                        success = SubsidiaryLedgerService.Delete(item);
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
    }
}