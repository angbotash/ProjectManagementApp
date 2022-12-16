using ProjectManagementApp.Domain.QueryOrder;

namespace ProjectManagementApp.Web.ViewModels
{
    public class UsersViewModel
    {
        public IList<UserViewModel> Users { get; set; } = new List<UserViewModel>();

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
