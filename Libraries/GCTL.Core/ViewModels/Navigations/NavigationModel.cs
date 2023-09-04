namespace GCTL.Core.ViewModels.Navigations
{
    public class NavigationModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ParentId { get; set; }
        public string Parent { get; set; }
        public string MenuId { get; set; }
        public int OrderBy { get; set; }
        public string ControllerName { get; set; }
        public string ViewName { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }

        public List<NavigationModel> Navigations { get; set; } = new List<NavigationModel>();
    }
}
