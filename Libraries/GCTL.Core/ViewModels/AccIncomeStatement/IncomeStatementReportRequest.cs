using GCTL.Core.Reports;

namespace GCTL.Core.ViewModels.AccIncomeStatement
{
    public  class IncomeStatementReportRequest : ApplicationReportRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
