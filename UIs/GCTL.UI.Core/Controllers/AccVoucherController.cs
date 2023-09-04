
using GCTL.Data.Models;
using GCTL.Service.Common;
using GCTL.Service.Companies;
using GCTL.Service.AccControlLedgers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GCTL.Core.Helpers;
using GCTL.Core.ViewModels.AccVouchers;
using GCTL.Service.AccVouchers;
using GCTL.Service.AccVoucherTypes;

using GCTL.Service.AccSubSubsidiaryLedgers;
using GCTL.UI.Core.ViewModels.AccVouchers;
using GCTL.Core.Select2;
using GCTL.UI.Core.Helpers.Reports;
using GCTL.Service.Reports;

namespace GCTL.UI.Core.Controllers
{
    public class AccVoucherController : BaseController
    {
        private readonly IAccVoucherService VoucherEntryService;
        private readonly IAccVoucherTypeService VoucherTypeService;
        private readonly IAccSubSubsidiaryLedgerService asslService;
        private readonly ICompanyService CompanyService;
        private readonly ICommonService commonService;
     
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IReportService reportService;
        string strMaxNO = "";
        public AccVoucherController(
            IAccVoucherService VoucherEntryService,
             IAccVoucherTypeService VoucherTypeService,
            IAccSubSubsidiaryLedgerService asslService,
            ICompanyService CompanyService,
                              ICommonService commonService,
                                IWebHostEnvironment webHostEnvironment,
                                 IReportService reportService,
                              IMapper mapper)
        {
            this.VoucherEntryService = VoucherEntryService;
            this.VoucherTypeService = VoucherTypeService;
            this.asslService = asslService;
            this.CompanyService = CompanyService;
            this.commonService = commonService;
            this.webHostEnvironment = webHostEnvironment;
            this.reportService = reportService;
            this.mapper = mapper;
        }
        //GetCompanyDropDown
        public IActionResult Index()
        {
            var TrType = new List<Select2Model>();
            TrType.Add(new Select2Model() { Id = "Dr", Text = "Dr" });
            TrType.Add(new Select2Model() { Id = "Cr", Text = "Cr" });

            AccVoucherPageViewModel model = new AccVoucherPageViewModel()
            {
                PageUrl = Url.Action(nameof(Index))
            };

            ViewBag.Company = new SelectList(CompanyService.GetCompanyDropDown(), "Code", "Name","001");
            ViewBag.VoucherType = new SelectList(VoucherTypeService.DropSelection(), "Code", "Name");
            ViewBag.LoadTrType = new SelectList(TrType, "Id", "Text");
            ViewBag.AccountHead = new SelectList(asslService.DropSelection(), "Code", "Name");
            return View(model);
        }


        public IActionResult Setup(string id)
        {
            var TrType = new List<Select2Model>();
            TrType.Add(new Select2Model() { Id = "Dr", Text = "Dr" });
            TrType.Add(new Select2Model() { Id = "Cr", Text = "Cr" });


            AccVoucherSetupViewModel model = new AccVoucherSetupViewModel();
            var result = VoucherEntryService.GetInfo(id);
            if (result != null)
            {
                model = mapper.Map<AccVoucherSetupViewModel>(result);
                model.Code = id;
            }
            ViewBag.Company = new SelectList(CompanyService.GetCompanyDropDown(), "Code", "Name");
            ViewBag.VoucherType = new SelectList(VoucherTypeService.DropSelection(), "Code", "Name");
            ViewBag.LoadTrType = new SelectList(TrType, "Id", "Text");
            ViewBag.AccountHead = new SelectList(asslService.DropSelection(), "Code", "Name");
            return PartialView($"_{nameof(Setup)}", model);
        }
        [HttpPost]      
        public IActionResult Update(AccVoucherSetupViewModel model)
        {

            if (ModelState.IsValid)
            {
                
                    var hasPermission = VoucherEntryService.UpdatePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                        VoucherEntryService.Update(model, LoginInfo);
                        return Json(new { isSuccess = true, message = "Update Successfully", lastCode = model.VoucherNo });
                    }
                    else
                    {
                        return Json(new { isSuccess = false, message = "You have no access" });
                    }
            }
            return Json(new { success = false, message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
        }
        [HttpPost]

        public IActionResult SaveData(AccVoucherSetupViewModel model)
        {
           
            if (ModelState.IsValid)
            {
               
                    var hasPermission = VoucherEntryService.SavePermission(LoginInfo.AccessCode);
                    if (hasPermission)
                    {
                      VoucherEntryService.Save(model, LoginInfo);
                      return Json(new { isSuccess = true, message = "Saved Successfully", lastCode = model.VoucherNo });
                    }
                    else
                    {

                        return Json(new { isSuccess = false, message = "You have no access" });
                    }
                
            }
            return Json(new { success = false, message = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage });
        }

        public ActionResult Grid(string FromDate,string ToDate)
        {
            var resutl = VoucherEntryService.GetInfoList(FromDate,ToDate);
            return Json(new { data = resutl });
        }
        public JsonResult MaxID(string VoucherType_Code)
        {
           
            commonService.FindVoucherNo(ref strMaxNO, VoucherType_Code);
            string MaxID = strMaxNO.ToString();
            return Json(MaxID);
        }
        public JsonResult GetAccountHeadDetails(string SubSusidiaryLedgerCodeNo)
        {
            var result = asslService.GetAccHeadGrid(SubSusidiaryLedgerCodeNo);
            return Json(result);
        }
        public JsonResult GetInfo(string VoucherNo)
        {
            var result = VoucherEntryService.GetInfo(VoucherNo);
            return Json(result);
        }
        public ActionResult GetInfoDetails(string VoucherNo)
        {
            var resutl = VoucherEntryService.GetInfoDetails(VoucherNo);
            return Json(resutl);
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            var hasPermission = VoucherEntryService.DeletePermission(LoginInfo.AccessCode);
            if (hasPermission)
            {
                bool success = false;
                var mes = "";
                foreach (var item in id.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    success = VoucherEntryService.Delete(item);
                    mes = "Deleted Successfully";
                }
                return Json(new { success = success, message = mes });
            }
            else
            {
                return Json(new { isSuccess = false, message = "You have no access" });
            }           
        }

        public ActionResult Export(AccVoucherReportRequest request)
        {
           
            request.ProcessingRequest(webHostEnvironment);
            var VoucherEntryReport = VoucherEntryService.GetInfoForReport(request.VoucherNo);

            var sources = new Dictionary<string, System.Collections.IEnumerable>
            {
                { "VoucherEntryReport",VoucherEntryReport }
               
            };
            request.Sources = sources;

            var reportResponse = reportService.GenerateReport(request);
            if (request.IsPreview)
            {
                return Json(reportResponse.PreviewPath);
            }
            else
            {
                return File(reportResponse.ReportResult.MainStream, reportResponse.MimeType, reportResponse.FileName + reportResponse.Extension);
            }
        }

    }
}