using GCTL.Core.ViewModels.PaymentTypes;
using GCTL.Data.Models;
using GCTL.Service.Common;
using GCTL.Service.PaymentTypes;
using GCTL.UI.Core.ViewModels.PaymentTypes;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GCTL.Core.Helpers;

namespace GCTL.UI.Core.Controllers
{
    public class PaymentTypesController : BaseController
    {
        private readonly IPaymentTypeService paymentTypeService;
        private readonly ICommonService commonService;
        private readonly IMapper mapper;
        string strMaxNO = "";
        public PaymentTypesController(IPaymentTypeService paymentTypeService,
                                      ICommonService commonService,
                                      IMapper mapper)
        {
            this.paymentTypeService = paymentTypeService;
            this.commonService = commonService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            PaymentTypePageViewModel model = new PaymentTypePageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };
            commonService.FindMaxNo(ref strMaxNO, "PaymentTypeId", "Sales_Def_PaymentType", 3);
            model.Setup = new PaymentTypeSetupViewModel
            {
                PaymentTypeId = strMaxNO
            };
            return View(model);
        }


        public IActionResult Setup(string id)
        {
            PaymentTypeSetupViewModel model = new PaymentTypeSetupViewModel();
            commonService.FindMaxNo(ref strMaxNO, "PaymentTypeId", "Sales_Def_PaymentType", 3);
            var result = paymentTypeService.GetPaymentType(id);
            if (result != null)
            {
                model = mapper.Map<PaymentTypeSetupViewModel>(result);
                model.Code = id;
            }
            else
            {
                model.PaymentTypeId = strMaxNO;
            }

            return PartialView($"_{nameof(Setup)}", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Setup(PaymentTypeSetupViewModel model)
        {
            if (paymentTypeService.IsPaymentTypeExist(model.PaymentType, model.PaymentTypeId))
            {
                return Json(new { isSuccess = false, message = "Already Exists" });
            }

            if (ModelState.IsValid)
            {
                SalesDefPaymentType paymentType = paymentTypeService.GetPaymentType(model.PaymentTypeId) ?? new  SalesDefPaymentType();
                model.ToAudit(LoginInfo, model.Tc > 0);
                mapper.Map(model, paymentType);
                paymentTypeService.SavePaymentType(paymentType);
                return Json(new { isSuccess = true, message = "Saved Successfully" });
            }

            return Json(new { success = false, message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
        }

        public ActionResult Grid()
        {
            var resutl = paymentTypeService.GetPaymentTypes();
            return Json(new { data = resutl });
        }


        [HttpPost]
        public ActionResult Delete(string id)
        {
            bool success = false;
            foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                success = paymentTypeService.DeletePaymentType(item);
            }

            return Json(new { success = success, message = "Deleted Successfully" });
        }


        [HttpPost]
        public JsonResult CheckAvailability(string name, string code)
        {
            if (paymentTypeService.IsPaymentTypeExist(name, code))
            {
                return Json(new { isSuccess = true, message = "Already Exists" });
            }

            return Json(new { isSuccess = false });
        }
    }
}