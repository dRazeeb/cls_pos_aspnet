using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCTL.Core.ViewModels.AccControlLedger
{
    public class AccControlLedgerSetupViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "GRL Code")]
        public string ControlLedgerCodeNo { get; set;}

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "GRL Name")]
        public string ControlLedgerName { get; set; }
        public string ShortName { get; set; }
    }
}
