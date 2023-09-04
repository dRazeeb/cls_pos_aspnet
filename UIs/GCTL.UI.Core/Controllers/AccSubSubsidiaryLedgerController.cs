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
using GCTL.UI.Core.ViewModels.AccSubSubsidiaryLedgers;
using GCTL.Core.ViewModels.AccGeneralLedgers;
using GCTL.Core.ViewModels.AccSubsidiaryLedgers;
using GCTL.Core.ViewModels.AccSubSubsidiaryLedgers;
using GCTL.Service.AccVouchers;

namespace GCTL.UI.Core.Controllers
{
    public class AccSubSubsidiaryLedgerController : BaseController
    {

        private readonly IAccSubSubsidiaryLedgerService SubSubsidiaryLedgerService;
        private readonly IAccSubsidiaryLedgerService SubsidiaryLedgerService;
        private readonly IAccGeneralLedgerService GeneralLedgerService;
        private readonly IAccSubControlLedgerService SubControlLedgerService;
        private readonly IAccControlLedgerService ControlLedgerService;
        private readonly IAccVoucherService VoucherEntryService;

        private readonly ICommonService commonService;        
        private readonly IMapper mapper;
        string strMaxNO = "";
        public AccSubSubsidiaryLedgerController(IAccSubSubsidiaryLedgerService SubSubsidiaryLedgerService,
            IAccSubsidiaryLedgerService SubsidiaryLedgerService, 
            IAccGeneralLedgerService GeneralLedgerService,
            IAccSubControlLedgerService SubControlLedgerService,
            IAccControlLedgerService ControlLedgerService,
             IAccVoucherService VoucherEntryService,
                              ICommonService commonService,
                              IMapper mapper)
        {
            this.SubSubsidiaryLedgerService = SubSubsidiaryLedgerService;
            this.SubsidiaryLedgerService = SubsidiaryLedgerService;
            this.GeneralLedgerService = GeneralLedgerService;
            this.ControlLedgerService = ControlLedgerService;
            this.SubControlLedgerService = SubControlLedgerService;
            this.VoucherEntryService = VoucherEntryService;
            this.commonService = commonService;           
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            AccSubSubsidiaryLedgerPageViewModel model = new AccSubSubsidiaryLedgerPageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };
            
            ViewBag.CL = new SelectList(ControlLedgerService.ControlLedgerSelection(), "Code", "Name");
            ViewBag.SCL = new SelectList(SubControlLedgerService.SubControlLedgerSelection(), "Code", "Name");
            ViewBag.GL = new SelectList(GeneralLedgerService.GeneralLedgerSelection(), "Code", "Name");
            ViewBag.SL = new SelectList(SubsidiaryLedgerService.DropSelection(), "Code", "Name");
            return View(model);
        }


        public IActionResult Setup(string id)
        {
            AccSubSubsidiaryLedgerSetupViewModel model = new AccSubSubsidiaryLedgerSetupViewModel();
            var result = SubSubsidiaryLedgerService.GetInfoForView(id);
            ViewBag.CL = new SelectList(ControlLedgerService.ControlLedgerSelection(), "Code", "Name");
            ViewBag.SCL = new SelectList(SubControlLedgerService.SubControlLedgerSelection(), "Code", "Name");
            ViewBag.GL = new SelectList(GeneralLedgerService.GeneralLedgerSelection(), "Code", "Name");
            ViewBag.SL = new SelectList(SubsidiaryLedgerService.DropSelection(), "Code", "Name");
            if (result != null)
            {
                model = mapper.Map<AccSubSubsidiaryLedgerSetupViewModel>(result);
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
        public IActionResult Setup(AccSubSubsidiaryLedgerSetupViewModel model)
        {
            if (SubSubsidiaryLedgerService.IsExist(model.SubsidiaryLedgerCodeNo,model.SubSubsidiaryLedgerName, model.SubSusidiaryLedgerCodeNo))
            {
                return Json(new { isSuccess = false, message = "Already Exists" });
            }

            if (ModelState.IsValid)
            {
                if (SubSubsidiaryLedgerService.IsExistByCode(model.SubSusidiaryLedgerCodeNo))
                {
                    var hasPermission = SubSubsidiaryLedgerService.UpdatePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        AccSubSubsidiaryLedger bed = SubSubsidiaryLedgerService.GetInfo(model.SubSusidiaryLedgerCodeNo) ?? new AccSubSubsidiaryLedger();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, bed);
                        SubSubsidiaryLedgerService.Save(bed);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = bed.SubSusidiaryLedgerCodeNo });
                    }
                    else
                    {

                        return Json(new { isSuccess = false, message = "You have no access" });
                    }

                }
                else
                {
                    var hasPermission = SubSubsidiaryLedgerService.SavePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        AccSubSubsidiaryLedger bed = SubSubsidiaryLedgerService.GetInfo(model.SubSusidiaryLedgerCodeNo) ?? new AccSubSubsidiaryLedger();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, bed);
                        SubSubsidiaryLedgerService.Save(bed);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = bed.SubSusidiaryLedgerCodeNo });
                    }
                    else
                    {

                        return Json(new { isSuccess = false, message = "You have no access" });
                    }
                }   
            }
            return Json(new { success = false, message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
        }

        public ActionResult Grid(string ControlLedgerCodeNo,string SubControlLedgerCodeNo,
            string GeneralLedgerCodeNo,string SubsidiaryLedgerCodeNo)
        {
            var resutl = SubSubsidiaryLedgerService.GetInfoList(ControlLedgerCodeNo,SubControlLedgerCodeNo,GeneralLedgerCodeNo, SubsidiaryLedgerCodeNo);
            return Json(new { data = resutl });
        }
        public JsonResult MaxID(string SubsidiaryLedgerCodeNo)
        {
            commonService.FindAccFourDigit(ref strMaxNO, "SubSusidiaryLedgerCodeNo", "Acc_SubSubsidiaryLedger", 5, "SubsidiaryLedgerCodeNo", SubsidiaryLedgerCodeNo);
            string MaxID = SubsidiaryLedgerCodeNo + strMaxNO.ToString();
            return Json(MaxID);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var hasPermission = SubSubsidiaryLedgerService.DeletePermission(LoginInfo.AccessCode);
            if (hasPermission)
            {

                bool success = false;
                var mes = "";
                foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    var entity = VoucherEntryService.IsASSLexistInVE(item);
                    if (entity)
                    {
                        mes = "Not Deleted. It Exist in Voucher Entry";
                    }
                    else
                    {
                        success = SubSubsidiaryLedgerService.Delete(item);
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