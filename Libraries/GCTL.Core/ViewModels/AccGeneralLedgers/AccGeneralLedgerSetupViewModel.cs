using System.ComponentModel.DataAnnotations;

namespace GCTL.Core.ViewModels.AccGeneralLedgers
{
    public class AccGeneralLedgerSetupViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "CL is required.")]
        [Display(Name = "CL")]
        public string SubControlLedgerCodeNo { get; set; }
        [Required(ErrorMessage = "SCL Code is required.")]
        [Display(Name = "SCL Code")]

        public string GeneralLedgerCodeNo { get; set; }
        [Required(ErrorMessage = "SCL Name is required.")]
        [Display(Name = "SCL Name")]

        public string GeneralLedgerName { get; set; }

        public string ShortName { get; set; }

        public string SubControlLedgerName { get; set; }
        public string ControlLedgerName { get; set; }
        public string ControlLedgerCodeNo { get; set; }

    }
}
