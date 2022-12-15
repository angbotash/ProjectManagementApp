using ProjectManagementApp.Domain.QueryOrder;

namespace ProjectManagementApp.Web.ViewModels
{
    public class ProjectsViewModel
    {
        public IList<ProjectViewModel> Projects { get; set; } = new List<ProjectViewModel>();

        public string? Filter { get; set; }

        public string? Order { get; set; }
        
        public SortDirection Direction { get; set; }

        public SortDirection GetNextSortDirection(string name, SortDirection defaultOrder)
        {
            if (Order?.ToLower() != name?.ToLower())
            {
                return defaultOrder;
            }

            return Direction == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
        }
    }
}
