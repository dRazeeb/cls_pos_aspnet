using GCTL.Core.ViewModels.ExpenseEntry;
using GCTL.Data.Models;
using GCTL.Service.Common;
using GCTL.Service.ExpenseEntry;
using GCTL.UI.Core.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GCTL.Core.Helpers;
using GCTL.UI.Core.ViewModels.ExpenseEntry;
using GCTL.Service.BankAccounts;
using GCTL.Service.AccSubSubsidiaryLedgers;
using GCTL.Service.PaymentModes;
using Microsoft.AspNetCore.Mvc.Rendering;
using GCTL.Service.AccVouchers;

namespace GCTL.UI.Core.Controllers
{
    public class ExpenseEntryController : BaseController
    {
        private readonly IExpenseEntryService ExpenseEntryService;
        private readonly IBankAccountService bankAccountService;
        private readonly IAccSubSubsidiaryLedgerService accASSLService;
        private readonly IPaymentModeService paymentModeService;
        private readonly IAccVoucherService accVoucherService;

        private readonly ICommonService commonService;
        private readonly IMapper mapper;
        string strMaxNO = "";
        public ExpenseEntryController(IExpenseEntryService ExpenseEntryService,
                                     IBankAccountService bankAccountService,
                                     IAccSubSubsidiaryLedgerService accASSLService,
                                     IPaymentModeService paymentModeService,
                                       IAccVoucherService accVoucherService,
                                     ICommonService commonService,
                                     IMapper mapper)
        {
            this.ExpenseEntryService = ExpenseEntryService;
            this.paymentModeService = paymentModeService;
            this.bankAccountService = bankAccountService;
            this.accASSLService = accASSLService;
            this.accVoucherService = accVoucherService;
            this.commonService = commonService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            ExpenseEntryPageViewModel model = new ExpenseEntryPageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };
            commonService.MaxNoWithYearAndTwoDight(ref strMaxNO, "ExpenseCode", "HMS_ExpenseEntry", 6, "EE");
            model.Setup = new ExpenseEntrySetupViewModel
            {
                ExpenseCode = strMaxNO
            };
            ViewBag.EH = new SelectList(accASSLService.GetInfoList(null, null, null, "3020010001"), "SubSusidiaryLedgerCodeNo", "SubSubsidiaryLedgerName");
            ViewBag.PaymentModes = new SelectList(paymentModeService.PaymentModeSelection(), "Code", "Name");           
            ViewBag.TransferBankAccounts = new SelectList(bankAccountService.BankAccountDetailsSelection(), "Code", "Name");
            ViewBag.BankFrom = new SelectList(bankAccountService.BankAccountDetailsSelection(), "Code", "Name");         
            ViewBag.BkashNo = new SelectList(accASSLService.getBkashkDropSelection(), "Code", "Name");

            return View(model);
        }


        public IActionResult Setup(string id)
        {
           
            ExpenseEntrySetupViewModel model = new ExpenseEntrySetupViewModel();
            commonService.MaxNoWithYearAndTwoDight(ref strMaxNO, "ExpenseCode", "HMS_ExpenseEntry",6, "EE");
            ViewBag.EH = new SelectList(accASSLService.GetInfoList(null, null, null, "3020010001"), "SubSusidiaryLedgerCodeNo", "SubSubsidiaryLedgerName");
            ViewBag.PaymentModes = new SelectList(paymentModeService.PaymentModeSelection(), "Code", "Name");
            ViewBag.TransferBankAccounts = new SelectList(bankAccountService.BankAccountDetailsSelection(), "Code", "Name");
            ViewBag.BankFrom = new SelectList(bankAccountService.BankAccountDetailsSelection(), "Code", "Name");
            ViewBag.BkashNo = new SelectList(accASSLService.getBkashkDropSelection(), "Code", "Name");
            var result = ExpenseEntryService.GetInfo(id);
            if (result != null)
            {
                model = mapper.Map<ExpenseEntrySetupViewModel>(result);
     
            }
            else
            {
                model.ExpenseCode = strMaxNO;
            }
           

            return PartialView($"_{nameof(Setup)}", model);
        }
        [HttpPost]
        public IActionResult Update(ExpenseEntrySetupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var hasPermission = ExpenseEntryService.UpdatePermission(LoginInfo.AccessCode);
                if (hasPermission)
                {
                    ExpenseEntryService.Update(model, LoginInfo);
                    bool isExcuteJV = ExpenseEntryService.ExcuteInVoucherEntryPV(model.ExpenseCode);
                    return Json(new { isSuccess = true, message = "Update Successfully", lastCode = model.ExpenseCode });
                }
                else
                {
                    return Json(new { isSuccess = false, message = "You have no access" });
                }
            }
            return Json(new { success = false, message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
        }

        public JsonResult GetInfo(string ExpenseCode)
        {
            var result = ExpenseEntryService.GetInfo(ExpenseCode);
            return Json(result);
        }
        [HttpPost]
        public IActionResult SaveData(ExpenseEntrySetupViewModel model)
        {
            
            if (ModelState.IsValid)
            {
               
                    var hasPermission = ExpenseEntryService.SavePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {

                        ExpenseEntryService.Save(model, LoginInfo);
                    bool isExcuteJV = ExpenseEntryService.ExcuteInVoucherEntryPV(model.ExpenseCode);
                    return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = model.ExpenseCode });
                    }
                    else
                    {

                        return Json(new { isSuccess = false, message = "You have no access" });
                    }
            }

            return Json(new { success = false, message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
        }

        public ActionResult Grid(string FromDate, string ToDate)
        {
            var result = ExpenseEntryService.GetInfoList(FromDate,ToDate);
            return Json(new { data = result });
        }

        public JsonResult MaxID()
        {
            commonService.MaxNoWithYearAndTwoDight(ref strMaxNO, "ExpenseCode", "HMS_ExpenseEntry", 6, "EE");
            
            string MaxID = strMaxNO.ToString();
            return Json(MaxID);
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            var hasPermission = ExpenseEntryService.DeletePermission(LoginInfo.AccessCode);
            if (hasPermission)
            {
                bool success = false;
                foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    success = ExpenseEntryService.Delete(item);
                    bool isExcute = accVoucherService.DeleteByInvoiceNo(item);
                }

                return Json(new { success = success, message = "Deleted Successfully" });
            }
            else
            {
                return Json(new { isSuccess = false, message = "You have no access" });
            }

           
        }

    }
}