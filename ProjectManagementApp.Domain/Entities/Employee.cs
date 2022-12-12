namespace ProjectManagementApp.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Patronymic { get; set; }

        public string Email { get; set; } = null!;

        public IList<EmployeeProject> EmployeeProject { get; set; } = null!;

        public IList<ProjectTask> Tasks { get; set; } = null!;
    }
}
