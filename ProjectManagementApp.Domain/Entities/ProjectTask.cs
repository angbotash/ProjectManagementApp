namespace ProjectManagementApp.Domain.Entities
{
    public class ProjectTask
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int ReporterId { get; set; }

        public Employee Reporter { get; set; } = null!;

        public int? AssigneeId { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; } = null!;

        public string? Comment { get; set; }

        public TaskStatus Status { get; set; }

        public int Priority { get; set; }
    }
}
