using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Web.ViewModels
{
    public class IssueViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int ReporterId { get; set; }

        public UserViewModel Reporter { get; set; } = null!;

        public int AssigneeId { get; set; }

        public UserViewModel Assignee { get; set; } = null!;

        public int ProjectId { get; set; }

        public ProjectViewModel Project { get; set; } = null!;

        public string? Comment { get; set; }

        public IssueStatus Status { get; set; }

        public int Priority { get; set; }
    }
}
