using ProjectManagementApp.Domain.QueryOrder;

namespace ProjectManagementApp.Web.ViewModels
{
    public class IssuesViewModel
    {
        public IList<IssueViewModel> Issues { get; set; } = new List<IssueViewModel>();

        public int? ProjectId;

        public int? ReporterId;

        public int? AsigneeId;

        public string? Order { get; set; }

        public SortDirection Direction { get; set; }

        public SortDirection GetNextSortDirection(string? name, SortDirection defaultOrder)
        {
            if (Order?.ToLower() != name?.ToLower())
            {
                return defaultOrder;
            }

            return Direction == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
        }
    }
}
