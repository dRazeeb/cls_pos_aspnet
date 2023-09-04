﻿using GCTL.Core.Data;
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
using GCTL.UI.Core.ViewModels.AccBalancesheet;
using GCTL.Core.ViewModels.AccBalancesheet;
using GCTL.Service.AccountReport;

namespace GCTL.UI.Core.Controllers
{
    public class BalancesheetController : BaseController
    {
        private readonly ICompanyService companyService;
        private readonly ICommonService commonService; 
        private readonly IAccountReportService accountReportService;
        private readonly IReportService reportService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IMapper mapper;
        public BalancesheetController(ICompanyService companyService,
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
            BalancesheetPageViewModel model = new BalancesheetPageViewModel();              
            return View(model);
        }

        public ActionResult Export(BalancesheetReportRequest request)
        {
            var BSFixedAsset= accountReportService.GetBSFixedAsset(request);
            var BSCurrentAsset = accountReportService.GetBSCurrentAsset(request);
            var BSNonCurrentLibilities = accountReportService.GetBSNonCurrentLibilities(request);
            var BSCurrentLibilities= accountReportService.GetBSCurrentLibilities(request);
      
            var BSCapital = accountReportService.GetBSCapital(request);
            var sources = new Dictionary<string, System.Collections.IEnumerable>
            {
                { "BSFixedAsset", BSFixedAsset },
                { "BSCurrentAsset", BSCurrentAsset },
                { "BSNonCurrentLibilities", BSNonCurrentLibilities },
                { "BSCurrentLibilities", BSCurrentLibilities },               
                { "BSCapital", BSCapital }
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