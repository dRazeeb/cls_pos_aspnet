using GCTL.Core.ViewModels;

namespace GCTL.UI.Core.ViewModels.Category
{
    public class CategoryTableViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string Description { get; set; }
        public bool? Status { get; set; }
        public string CreatedByName { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
