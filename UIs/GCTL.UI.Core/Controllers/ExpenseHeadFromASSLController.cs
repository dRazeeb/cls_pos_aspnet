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
    public class ExpenseHeadFromASSLController : BaseController
    {

        private readonly IAccSubSubsidiaryLedgerService SubSubsidiaryLedgerService;
        private readonly IAccVoucherService VoucherEntryService;
        private readonly ICommonService commonService;        
        private readonly IMapper mapper;
        string strMaxNO = "";
        public ExpenseHeadFromASSLController(IAccSubSubsidiaryLedgerService SubSubsidiaryLedgerService,
           IAccVoucherService VoucherEntryService,
                              ICommonService commonService,
                              IMapper mapper)
        {
            this.SubSubsidiaryLedgerService = SubSubsidiaryLedgerService;
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

          
            commonService.FindAccFourDigit(ref strMaxNO, "SubSusidiaryLedgerCodeNo", "Acc_SubSubsidiaryLedger", 5, "SubsidiaryLedgerCodeNo", "3020010001");
            string MaxID = "3020010001" + strMaxNO.ToString();
            model.Setup = new AccSubSubsidiaryLedgerSetupViewModel
            {
                SubSusidiaryLedgerCodeNo = MaxID
            };

            return View(model);
        }


        public IActionResult Setup(string id)
        {
            AccSubSubsidiaryLedgerSetupViewModel model = new AccSubSubsidiaryLedgerSetupViewModel();
            var result = SubSubsidiaryLedgerService.GetInfoForView(id);
          
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
                    
                        AccSubSubsidiaryLedger bed = SubSubsidiaryLedgerService.GetInfo(model.SubSusidiaryLedgerCodeNo) ?? new AccSubSubsidiaryLedger();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, bed);
                        SubSubsidiaryLedgerService.Save(bed);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = bed.SubSusidiaryLedgerCodeNo });
                   

                }
                else
                {
                    
                        AccSubSubsidiaryLedger bed = SubSubsidiaryLedgerService.GetInfo(model.SubSusidiaryLedgerCodeNo) ?? new AccSubSubsidiaryLedger();
                        model.ToAudit(LoginInfo, model.Id > 0);
                        mapper.Map(model, bed);
                        SubSubsidiaryLedgerService.Save(bed);
                        return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = bed.SubSusidiaryLedgerCodeNo });
                  
                }   
            }
            return Json(new { success = false, message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
        }

        public ActionResult Grid()
        {
            var resutl = SubSubsidiaryLedgerService.GetInfoList(null, null, null, "3020010001");
            return Json(new { data = resutl });
        }
        public JsonResult MaxID(string SubsidiaryLedgerCodeNo)
        {
            commonService.FindAccFourDigit(ref strMaxNO, "SubSusidiaryLedgerCodeNo", "Acc_SubSubsidiaryLedger", 5, "SubsidiaryLedgerCodeNo", "3020010001");
            string MaxID = SubsidiaryLedgerCodeNo + strMaxNO.ToString();
            return Json(MaxID);
        }

        [HttpPost]
        public ActionResult Delete(string id)
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
    }
}