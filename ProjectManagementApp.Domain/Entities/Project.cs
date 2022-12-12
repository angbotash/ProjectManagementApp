namespace ProjectManagementApp.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string ClientCompanyName { get; set; } = null!;

        public string ExecutorCompanyName { get; set; } = null!;

        public IList<EmployeeProject> EmployeeProject { get; set; } = null!;

        public Employee? Manager { get; set; }

        public int? ManagerId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Priority { get; set; }

        public IList<ProjectTask> Tasks { get; set; } = null!;
    }
}
