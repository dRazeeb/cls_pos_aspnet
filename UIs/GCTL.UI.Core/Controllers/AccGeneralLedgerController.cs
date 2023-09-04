using GCTL.Data.Models;
using GCTL.Service.Common;
using GCTL.Service.AccControlLedgers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GCTL.Core.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using GCTL.Service.AccSubControlLedgers;
using GCTL.Service.AccGeneralLedgers;
using GCTL.Service.AccSubsidiaryLedgers;


using GCTL.UI.Core.ViewModels.AccGeneralLedgers;
using GCTL.Core.ViewModels.AccGeneralLedgers;


namespace GCTL.UI.Core.Controllers
{
    public class AccGeneralLedgerController : BaseController
    {

        private readonly IAccGeneralLedgerService GeneralLedgerService;
        private readonly IAccSubControlLedgerService SubControlLedgerService;
        private readonly IAccControlLedgerService ControlLedgerService;
        private readonly IAccSubsidiaryLedgerService SubsidiaryLedgerService;
        private readonly ICommonService commonService;        
        private readonly IMapper mapper;
        string strMaxNO = "";
        public AccGeneralLedgerController(IAccGeneralLedgerService GeneralLedgerService,
            IAccSubControlLedgerService SubControlLedgerService,
            IAccControlLedgerService ControlLedgerService,
             IAccSubsidiaryLedgerService SubsidiaryLedgerService,
                              ICommonService commonService,
                              IMapper mapper)
        {
            this.GeneralLedgerService = GeneralLedgerService;
            this.ControlLedgerService = ControlLedgerService;
            this.SubControlLedgerService = SubControlLedgerService;
            this.SubsidiaryLedgerService = SubsidiaryLedgerService;
            this.commonService = commonService;           
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            AccGeneralLedgerPageViewModel model = new AccGeneralLedgerPageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };
            
            ViewBag.CL = new SelectList(ControlLedgerService.ControlLedgerSelection(), "Code", "Name");
            ViewBag.SCL = new SelectList(SubControlLedgerService.SubControlLedgerSelection(), "Code", "Name");

            return View(model);
        }


        public IActionResult Setup(string id)
        {
            AccGeneralLedgerSetupViewModel model = new AccGeneralLedgerSetupViewModel();
            var result = GeneralLedgerService.GetGeneralLedgerForView(id);
            ViewBag.CL = new SelectList(ControlLedgerService.ControlLedgerSelection(), "Code", "Name");
            ViewBag.SCL = new SelectList(SubControlLedgerService.SubControlLedgerSelection(), "Code", "Name");

            if (result != null)
            {
                model = mapper.Map<AccGeneralLedgerSetupViewModel>(result);
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
        public IActionResult Setup(AccGeneralLedgerSetupViewModel model)
        {
            if (GeneralLedgerService.IsGeneralLedgerExist(model.SubControlLedgerCodeNo,model.GeneralLedgerName, model.GeneralLedgerCodeNo))
            {
                return Json(new { isSuccess = false, message = "Already Exists" });
            }

            if (ModelState.IsValid)
            {
                if (GeneralLedgerService.IsGeneralLedgerExistByCode(model.GeneralLedgerCodeNo))
                {
                    var hasPermission = GeneralLedgerService.UpdatePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        AccGeneralLedger bed = GeneralLedgerService.GetGeneralLedger(model.GeneralLedgerCodeNo) ?? new AccGeneralLedger();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, bed);
                        GeneralLedgerService.SaveGeneralLedger(bed);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = bed.GeneralLedgerCodeNo });
                    }
                    else
                    {

                        return Json(new { isSuccess = false, message = "You have no access" });
                    }

                }
                else
                {
                    var hasPermission = GeneralLedgerService.SavePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        AccGeneralLedger bed = GeneralLedgerService.GetGeneralLedger(model.GeneralLedgerCodeNo) ?? new AccGeneralLedger();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, bed);
                        GeneralLedgerService.SaveGeneralLedger(bed);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = bed.GeneralLedgerCodeNo });
                    }
                    else
                    {

                        return Json(new { isSuccess = false, message = "You have no access" });
                    }
                }   
            }
            return Json(new { success = false, message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
        }

        public ActionResult Grid(string ControlLedgerCodeNo,string SubControlLedgerCodeNo)
        {
            var resutl = GeneralLedgerService.GetGeneralLedgers(ControlLedgerCodeNo,SubControlLedgerCodeNo);
            return Json(new { data = resutl });
        }
        public JsonResult MaxID(string SubControlLedgerCodeNo)
        {
            commonService.FindAccThreeDigit(ref strMaxNO, "GeneralLedgerCodeNo", "Acc_GeneralLedger", 3, "SubControlLedgerCodeNo", SubControlLedgerCodeNo);
            string MaxID = SubControlLedgerCodeNo + strMaxNO.ToString();
            return Json(MaxID);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var hasPermission = GeneralLedgerService.DeletePermission(LoginInfo.AccessCode);
            if (hasPermission)
            {

                bool success = false;
                var mes = "";
                foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    var entity = SubsidiaryLedgerService.IsExistByGL(item);
                    if (entity)
                    {
                        mes = "Not Deleted. It Exist in Subsidiary Ledger";
                    }
                    else
                    {
                        success = GeneralLedgerService.DeleteGeneralLedger(item);
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