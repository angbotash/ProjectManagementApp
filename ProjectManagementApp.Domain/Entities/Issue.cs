using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Domain.Entities
{
    public class Issue
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Name { get; set; } = null!;

        public int AssigneeId { get; set; }

        public User Assignee { get; set; } = null!;

        public int ReporterId { get; set; }

        public User Reporter { get; set; } = null!;

        public int ProjectId { get; set; }

        public Project Project { get; set; } = null!;

        [StringLength(1000)]
        public string? Comment { get; set; }

        public IssueStatus Status { get; set; }

        [Range(1, 10)]
        [Required]
        public int Priority { get; set; }
    }
}
