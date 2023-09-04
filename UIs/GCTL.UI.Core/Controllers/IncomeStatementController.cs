using GCTL.Core.Data;
using GCTL.Data.Models;
using GCTL.Service.Common;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GCTL.Service.Reports;
using GCTL.UI.Core.Helpers.Reports;
using GCTL.Core.ViewModels.Companies;
using GCTL.Service.Companies;
using System.Text;
using GCTL.UI.Core.ViewModels.CashbookReport;
using GCTL.Core.ViewModels.CashbookReport;
using GCTL.Core;
using GCTL.UI.Core.ViewModels.AccIncomeStatement;
using GCTL.Core.ViewModels.AccIncomeStatement;
using GCTL.Service.AccountReport;

namespace GCTL.UI.Core.Controllers
{
    public class IncomeStatementController : BaseController
    {
        private readonly ICompanyService companyService;
        private readonly ICommonService commonService; 
        private readonly IAccountReportService accountReportService;
        private readonly IReportService reportService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IMapper mapper;
        public IncomeStatementController(ICompanyService companyService,
            ICommonService commonService,                                     
                                       IAccountReportService accountReportService,
                                       IReportService reportService,
                                       IWebHostEnvironment webHostEnvironment,
                                       IMapper mapper)
        {
            this.companyService = companyService;
            this.commonService = commonService;          
            this.accountReportService = accountReportService;
            this.reportService = reportService;
            this.webHostEnvironment = webHostEnvironment;
            this.mapper = mapper;
        }

        public ActionResult Index()
        {
            IncomeStatementPageViewModel model = new IncomeStatementPageViewModel();              
            return View(model);
        }

        public ActionResult Export(IncomeStatementReportRequest request)
        {
            var PLTotalSales = accountReportService.IncServiceRevenue(request);
            var PLTotalOtherIncome = accountReportService.IncOtherIncome(request);
            var PLTotalDirectExpense = accountReportService.PLTotalDirectExpense(request);
            var PLTotalInDirectExpense = accountReportService.PLTotalInDirectExpense(request);

            var sources = new Dictionary<string, System.Collections.IEnumerable>
            {
                { "PLTotalSales", PLTotalSales },
                { "PLTotalOtherIncome", PLTotalOtherIncome },
                { "PLTotalDirectExpense", PLTotalDirectExpense },
                { "PLTotalInDirectExpense", PLTotalInDirectExpense }
            };

            request.Sources = sources;
            request.ProcessingRequest(webHostEnvironment);
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