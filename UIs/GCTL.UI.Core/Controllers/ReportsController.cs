using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using GCTL.Core.Reports;
using GCTL.Service.Helpers;
using GCTL.Service.Reports;
using GCTL.UI.Core.Helpers.Reports;
using System.Collections;
using System.Net.Mime;
using System.Text;

namespace GCTL.UI.Core.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportService reportService;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ReportsController(IReportService reportService,
                                 IWebHostEnvironment webHostEnvironment)
        {
            this.reportService = reportService;
            this.webHostEnvironment = webHostEnvironment;
        }

        public ActionResult Export(ApplicationReportRequest request)
        {
            request.ProcessingRequest(webHostEnvironment);
            request.SetSource(GetMoneyReceipts());

            var reportResponse = reportService.GenerateReport(request);
            return File(reportResponse.ReportResult.MainStream, reportResponse.MimeType, reportResponse.FileName + reportResponse.Extension);
        }

       



        private IEnumerable GetMoneyReceipts()
        {
            List<Designation> designations = new List<Designation>();
            designations.Add(new Designation
            {
                AutoId = "1",
                DesignationCode = "100",
                DesignationName = "Manager",
                ShortName = "M",
                Date = DateTime.Now.ToShortDateString()
            });

            designations.Add(new Designation
            {
                AutoId = "2",
                DesignationCode = "200",
                DesignationName = "Software Engineer",
                ShortName = "SE",
                Date = DateTime.Now.ToShortDateString()
            });

            designations.Add(new Designation
            {
                AutoId = "3",
                DesignationCode = "300",
                DesignationName = "System Analyst",
                ShortName = "SA",
                Date = DateTime.Now.ToShortDateString()
            });

            return designations;
        }

       


        //public IActionResult Print1()
        //{
        //    string minetype = "";
        //    int extension = 1;
        //    var rdlcFilePath = $"{this._webHostEnvironment.WebRootPath}\\Report\\Report1.rdlc";
        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        //    Encoding.GetEncoding("windows-1252");
        //    LocalReport report = new LocalReport(rdlcFilePath);
        //    var data = vendorInfoService.GetVendorInfos();
        //    report.AddDataSource("DataSet1", data);
        //    var result = report.Execute(GetRenderType("excel"), extension, parameters, minetype);
        //    return File(result.MainStream, "application/msexcel", "Export.xls");
        //}

        //public byte[] GenerateReportAsync(string reportName)
        //{
        //    reportName = "Department";
        //    // string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("ReportAPI.dll", string.Empty);
        //    var rdlcFilePath = $"{this.webHostEnvironment.WebRootPath}\\reports\\{reportName}.rdlc";
        //    // var rdlcFilePath = $"{this.webHostEnvironment.WebRootPath}\\reports\\{reportName}.rdlc";
        //    // string rdlcFilePath = string.Format("{0}ReportFiles\\{1}.rdlc", fileDirPath, reportName);
        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        //    Encoding.GetEncoding("windows-1252");
        //    LocalReport report = new LocalReport(rdlcFilePath);


        //    List<Designation> designations = new List<Designation>();
        //    designations.Add(new Designation
        //    {
        //        AutoId = "1",
        //        DesignationCode = "100",
        //        DesignationName = "Manager",
        //        ShortName = "M",
        //        Date = DateTime.Now.ToShortDateString()
        //    });

        //    designations.Add(new Designation
        //    {
        //        AutoId = "2",
        //        DesignationCode = "200",
        //        DesignationName = "Software Engineer",
        //        ShortName = "SE",
        //        Date = DateTime.Now.ToShortDateString()
        //    });

        //    designations.Add(new Designation
        //    {
        //        AutoId = "3",
        //        DesignationCode = "300",
        //        DesignationName = "System Analyst",
        //        ShortName = "SA",
        //        Date = DateTime.Now.ToShortDateString()
        //    });

        //    report.AddDataSource(reportName, designations);

        //    //  report.AddDataSource("dtsetDes", designations);
        //    parameters.Add("Title", "Jamal Uddin");
        //    var result = report.Execute(GetRenderType("pdf"), 1, parameters);
        //    return result.MainStream;
        //}


    }



  

    

    
    public class Designation
    {
        public string AutoId { get; set; }
        public string DesignationCode { get; set; }
        public string DesignationName { get; set; }
        public string ShortName { get; set; }
        public string Date { get; set; }
    }
}
