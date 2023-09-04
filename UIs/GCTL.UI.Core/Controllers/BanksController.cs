using GCTL.Core.ViewModels.Banks;
using GCTL.Data.Models;
using GCTL.Service.Common;
using GCTL.Service.Banks;
using GCTL.UI.Core.ViewModels.Banks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GCTL.Core.Helpers;

namespace GCTL.UI.Core.Controllers
{
    public class BanksController : BaseController
    {
        private readonly IBankService bankService;
        private readonly ICommonService commonService;
        private readonly IMapper mapper;
        string strMaxNO = "";
        public BanksController(IBankService bankService,
                                     ICommonService commonService,
                                     IMapper mapper)
        {
            this.bankService = bankService;
            this.commonService = commonService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            BankPageViewModel model = new BankPageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };
            commonService.FindMaxNo(ref strMaxNO, "BankId", "SALES_Def_BankInfo", 3);
            model.Setup = new BankSetupViewModel
            {
                BankId = strMaxNO
            };

            return View(model);
        }


        public IActionResult Setup(string id)
        {
            BankSetupViewModel model = new BankSetupViewModel();
            commonService.FindMaxNo(ref strMaxNO, "BankId", "SALES_Def_BankInfo", 3);
            var result = bankService.GetBank(id);
            if (result != null)
            {
                model = mapper.Map<BankSetupViewModel>(result);
                model.Code = id;
                model.AutoId = (int)result.AutoId;
            }
            else
            {
                model.BankId = strMaxNO;
            }

            return PartialView($"_{nameof(Setup)}", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Setup(BankSetupViewModel model)
        {
            if (bankService.IsBankExist(model.BankName, model.BankId))
            {
                return Json(new { isSuccess = false, message = "Already Exists" });
            }

            if (ModelState.IsValid)
            {
                SalesDefBankInfo bank = bankService.GetBank(model.BankId) ?? new SalesDefBankInfo();
                model.ToAudit(LoginInfo, model.AutoId > 0);
                mapper.Map(model, bank);
                bankService.SaveBank(bank);
                return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = bank.BankId });
            }

            return Json(new { success = false, message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
        }

        public ActionResult Grid()
        {
            var result = bankService.GetBanks();
            return Json(new { data = result });
        }


        [HttpPost]
        public ActionResult Delete(string id)
        {
            bool success = false;
            foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                success = bankService.DeleteBank(item);
            }

            return Json(new { success = success, message = "Deleted Successfully" });
        }


        [HttpPost]
        public JsonResult CheckAvailability(string name, string code)
        {
            if (bankService.IsBankExist(name, code))
            {
                return Json(new { isSuccess = true, message = "Already Exists" });
            }

            return Json(new { isSuccess = false });
        }
    }
}