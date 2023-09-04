using System.ComponentModel.DataAnnotations;

namespace GCTL.Core.ViewModels.Designations
{
    public class DesignationSetupViewModel : BaseViewModel
    {
        public string DesignationCode { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Display(Name = "Designation Name")]
        public string DesignationName { get; set; }

        [Display(Name = "Designaiton Short Name")]
        public string DesignationShortName { get; set; }

        [Display(Name = "Designaiton Short Name")]
        public string BanglaDesignation { get; set; }
    }
}
