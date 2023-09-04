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

namespace GCTL.UI.Core.Controllers
{
    public class CashbookReportController : BaseController
    {
        private readonly ICompanyService companyService;
        private readonly ICommonService commonService; 
        private readonly IReportService reportService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IMapper mapper;
        public CashbookReportController(ICompanyService companyService,
            ICommonService commonService,                                     
                                      
                                       IReportService reportService,
                                       IWebHostEnvironment webHostEnvironment,
                                       IMapper mapper)
        {
            this.companyService = companyService;
            this.commonService = commonService;          
            this.reportService = reportService;
            this.webHostEnvironment = webHostEnvironment;
            this.mapper = mapper;
        }

        public ActionResult Index()
        {
            CashbookReportPageViewModel model = new CashbookReportPageViewModel();
              
            return View(model);
        }

        //public ActionResult Export(CashbookReportRequest request)
        //{
        //    var collections = billEntryService.CashbookReport(request);

        //    var sources = new Dictionary<string, System.Collections.IEnumerable>
        //    {
        //        { "CashbookReport", collections }
        //    };
        //    request.Sources = sources;
        //    request.ProcessingRequest(webHostEnvironment);
        //    var reportResponse = reportService.GenerateReport(request);
        //    if (request.IsPreview)
        //    {
        //        return Json(reportResponse.PreviewPath);
        //    }
        //    else
        //    {
        //        return File(reportResponse.ReportResult.MainStream, reportResponse.MimeType, reportResponse.FileName + reportResponse.Extension);
        //    }
        //}
    }
}