using System.ComponentModel.DataAnnotations;


namespace GCTL.Core.ViewModels.AccSubsidiaryLedgers
{
    public class AccSubsidiaryLedgerSetupViewModel : BaseViewModel
    {

        [Required(ErrorMessage = "SCL is required.")]
        [Display(Name = "SCL")]
        public string GeneralLedgerCodeNo { get; set; }

        [Required(ErrorMessage = "SSL Code is required.")]
        [Display(Name = "SSL")]
        public string SusidiaryLedgerCodeNo { get; set; }

        [Required(ErrorMessage = "SSL Name is required.")]
        [Display(Name = "SSL Name")]
        public string SubsidiaryLedgerName { get; set; }
        public string ShortName { get; set; }

        public string GeneralLedgerName { get; set; }
        public string SubControlLedgerCodeNo { get; set; }
        public string SubControlLedgerName { get; set; }
        public string ControlLedgerName { get; set; }
        public string ControlLedgerCodeNo { get; set; }
    }
}
