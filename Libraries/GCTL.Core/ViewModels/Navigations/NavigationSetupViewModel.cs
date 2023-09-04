using System.ComponentModel.DataAnnotations;

namespace GCTL.Core.ViewModels.Navigations
{
    public class NavigationSetupViewModel : BaseViewModel
    {
        public int AutoId { get; set; }
        public string MenuId { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string Title { get; set; }

        [Display(Name ="Module")]
        public string ParentId { get; set; }  
        public int OrderBy { get; set; }
        public string ControllerName { get; set; }
        public string ViewName { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
    }
}
