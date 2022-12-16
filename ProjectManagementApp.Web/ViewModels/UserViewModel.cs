namespace ProjectManagementApp.Web.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Patronymic { get; set; }

        public string Email { get; set; } = null!;

        public IList<ProjectViewModel> Projects { get; set; } = new List<ProjectViewModel>();

        //public IList<IssueViewModel> AssignedIssues { get; set; } = new List<IssueViewModel>();

        //public IList<IssueViewModel> ReportedIssues { get; set; } = new List<IssueViewModel>();

    }
}
