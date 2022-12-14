namespace ProjectManagementApp.Web.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string ClientCompanyName { get; set; } = null!;

        public string ExecutorCompanyName { get; set; } = null!;

        public UserViewModel? Manager { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Priority { get; set; }

        public IList<UserViewModel> Users { get; set; } = new List<UserViewModel>();

        public IList<IssueViewModel> Issues { get; set; } = new List<IssueViewModel>();
    }
}
