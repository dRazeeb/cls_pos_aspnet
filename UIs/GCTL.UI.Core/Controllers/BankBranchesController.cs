using GCTL.Data.Models;
using GCTL.Service.Common;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GCTL.Core.Helpers;
using GCTL.Service.BankBranches;
using GCTL.UI.Core.ViewModels.BankBranches;
using GCTL.Core.ViewModels.BankBranches;
using GCTL.Service.Banks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCTL.UI.Core.Controllers
{
    public class BankBranchesController : BaseController
    {
        private readonly IBankBranchService bankBranchService;
        private readonly IBankService bankService;
        private readonly ICommonService commonService;
        private readonly IMapper mapper;
        string strMaxNO = "";
        public BankBranchesController(IBankBranchService bankBranchService,
                                      IBankService bankService,
                                     ICommonService commonService,
                                     IMapper mapper)
        {
            this.bankBranchService = bankBranchService;
            this.bankService = bankService;
            this.commonService = commonService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            BankBranchPageViewModel model = new BankBranchPageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };
            commonService.FindMaxNo(ref strMaxNO, "BankBranchId", "Sales_Def_BankBranchInfo", 4);
            model.Setup = new BankBranchSetupViewModel
            {
                BankBranchId = strMaxNO
            };

            ViewBag.Banks = new SelectList(bankService.BankSelection(), "Code", "Name");
            return View(model);
        }


        public IActionResult Setup(string id)
        {
            BankBranchSetupViewModel model = new BankBranchSetupViewModel();
            commonService.FindMaxNo(ref strMaxNO, "BankBranchId", "Sales_Def_BankBranchInfo", 4);
            var result = bankBranchService.GetBankBranch(id);
            if (result != null)
            {
                model = mapper.Map<BankBranchSetupViewModel>(result);
                model.Code = id;
                model.AutoId = (int)result.AutoId;
            }
            else
            {
                model.BankBranchId = strMaxNO;
            }

            ViewBag.Banks = new SelectList(bankService.BankSelection(), "Code", "Name", model.BankId);
            return PartialView($"_{nameof(Setup)}", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Setup(BankBranchSetupViewModel model)
        {
            if (bankBranchService.IsBankBranchExist(model.BankBranchName, model.BankBranchId))
            {
                return Json(new { isSuccess = false, message = "Already Exists" });
            }

            if (ModelState.IsValid)
            {
                SalesDefBankBranchInfo bankBranch = bankBranchService.GetBankBranch(model.BankBranchId) ?? new SalesDefBankBranchInfo();
                model.ToAudit(LoginInfo, model.AutoId > 0);
                mapper.Map(model, bankBranch);
                bankBranchService.SaveBankBranch(bankBranch);
                return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = bankBranch.BankBranchId });
            }

            return Json(new { success = false, message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
        }

        public ActionResult Grid()
        {
            var result = bankBranchService.GetBankBranches();
            return Json(new { data = result });
        }


        [HttpPost]
        public ActionResult Delete(string id)
        {
            bool success = false;
            foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                success = bankBranchService.DeleteBankBranch(item);
            }

            return Json(new { success = success, message = "Deleted Successfully" });
        }


        [HttpPost]
        public JsonResult CheckAvailability(string name, string code)
        {
            if (bankBranchService.IsBankBranchExist(name, code))
            {
                return Json(new { isSuccess = true, message = "Already Exists" });
            }

            return Json(new { isSuccess = false });
        }
    }
}