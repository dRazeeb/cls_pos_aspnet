using GCTL.Data.Models;
using GCTL.Service.Common;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GCTL.Core.Helpers;
using GCTL.Service.Banks;
using Microsoft.AspNetCore.Mvc.Rendering;
using GCTL.UI.Core.ViewModels.BankAccounts;
using GCTL.Core.ViewModels.BankAccounts;
using GCTL.Service.BankAccounts;
using GCTL.Service.BankBranches;

namespace GCTL.UI.Core.Controllers
{
    public class BankAccountsController : BaseController
    {
        private readonly IBankAccountService bankAccountService;
        private readonly IBankService bankService;
        private readonly IBankBranchService bankBranchService;
        private readonly ICommonService commonService;
        private readonly IMapper mapper;
        string strMaxNO = "";
        public BankAccountsController(IBankAccountService bankAccountService,
                                      IBankService bankService,
                                      IBankBranchService bankBranchService,
                                     ICommonService commonService,
                                     IMapper mapper)
        {
            this.bankAccountService = bankAccountService;
            this.bankService = bankService;
            this.bankBranchService = bankBranchService;
            this.commonService = commonService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            BankAccountPageViewModel model = new BankAccountPageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };
            commonService.FindMaxNo(ref strMaxNO, "AccInfoID", "Core_BankAccountInformation", 3);
            model.Setup = new BankAccountSetupViewModel
            {
                AccInfoId = strMaxNO
            };

            ViewBag.Banks = new SelectList(bankService.BankSelection(), "Code", "Name");
            ViewBag.Branches = new SelectList(bankBranchService.BankBranchSelection(), "Code", "Name");
            return View(model);
        }


        public IActionResult Setup(string id)
        {
            BankAccountSetupViewModel model = new BankAccountSetupViewModel();
            commonService.FindMaxNo(ref strMaxNO, "AccInfoID", "Core_BankAccountInformation", 3);
            var result = bankAccountService.GetBankAccount(id);
            if (result != null)
            {
                model = mapper.Map<BankAccountSetupViewModel>(result);
                model.Code = id;
                model.AutoId = (int)result.AutoId;
            }
            else
            {
                model.AccInfoId = strMaxNO;
            }

            ViewBag.Banks = new SelectList(bankService.BankSelection(), "Code", "Name", model.BankId);
            ViewBag.Branches = new SelectList(bankBranchService.BankBranchSelection(), "Code", "Name", model.BranchId);
            return PartialView($"_{nameof(Setup)}", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Setup(BankAccountSetupViewModel model)
        {
            if (bankAccountService.IsBankAccountExist(model.AccountName, model.AccInfoId))
            {
                return Json(new { isSuccess = false, message = "Already Exists" });
            }

            if (ModelState.IsValid)
            {
                CoreBankAccountInformation bankAccount = bankAccountService.GetBankAccount(model.AccountName) ?? new CoreBankAccountInformation();
                model.ToAudit(LoginInfo, model.AutoId > 0);
                mapper.Map(model, bankAccount);
                bankAccount.CompanyCode = "001";
                bankAccountService.SaveBankAccount(bankAccount);
                return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = bankAccount.AccInfoId });
            }

            return Json(new { success = false, message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
        }

        public ActionResult Grid()
        {
            var result = bankAccountService.GetBankAccounts();
            return Json(new { data = result });
        }


        [HttpPost]
        public ActionResult Delete(string id)
        {
            bool success = false;
            foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                success = bankAccountService.DeleteBankAccount(item);
            }

            return Json(new { success = success, message = "Deleted Successfully" });
        }


        [HttpPost]
        public JsonResult CheckAvailability(string name, string code)
        {
            if (bankAccountService.IsBankAccountExist(name, code))
            {
                return Json(new { isSuccess = true, message = "Already Exists" });
            }

            return Json(new { isSuccess = false });
        }
    }
}