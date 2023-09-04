using System.ComponentModel.DataAnnotations;


namespace GCTL.Core.ViewModels.AccSubSubsidiaryLedgers
{
    public class AccSubSubsidiaryLedgerSetupViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "SSL is required.")]
        [Display(Name = "SSL")]
        public string SubsidiaryLedgerCodeNo { get; set; }

        [Required(ErrorMessage = "GL Code is required.")]
        [Display(Name = "GL Code")]
        public string SubSusidiaryLedgerCodeNo { get; set; }

        [Required(ErrorMessage = "GL Name is required.")]
        [Display(Name = "GL Name")]
        public string SubSubsidiaryLedgerName { get; set; }
        public string ShortName { get; set; }
        public decimal? OpeningBalance { get; set; }


        public string SubsidiaryLedgerName { get; set; }
        public string GeneralLedgerCodeNo { get; set; }
        public string GeneralLedgerName { get; set; }
        public string SubControlLedgerCodeNo { get; set; }
        public string SubControlLedgerName { get; set; }
        public string ControlLedgerName { get; set; }
        public string ControlLedgerCodeNo { get; set; }
    }
}
