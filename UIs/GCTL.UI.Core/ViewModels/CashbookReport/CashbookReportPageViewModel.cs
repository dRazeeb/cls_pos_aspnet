using GCTL.Core;
using GCTL.Core.ViewModels;
using System.ComponentModel.DataAnnotations;


namespace GCTL.UI.Core.ViewModels.CashbookReport
{
    public class CashbookReportPageViewModel : BaseViewModel
    {
        [Display(Name = "Bill Type")]
        public string BillTypeId { get; set; }
        public string FromDate { get; set; } = DateTime.Now.ToString(ApplicationConstants.DateFormat);
        public string ToDate { get; set; } = DateTime.Now.ToString(ApplicationConstants.DateFormat);
    }
}
