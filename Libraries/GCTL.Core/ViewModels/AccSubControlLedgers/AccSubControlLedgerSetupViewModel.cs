using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCTL.Core.ViewModels.AccSubControlLedgers
{
    public class AccSubControlLedgerSetupViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "GRL Code")]
        public string ControlLedgerCodeNo { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "CL Code")]
        public string SubControlLedgerCodeNo { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string SubControlLedgerName { get; set; }
        public string ShortName { get; set; }
        public string ControlLedgerName { get; set; }

    }
}
