using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string ClientCompanyName { get; set; } = null!;

        public string ExecutorCompanyName { get; set; } = null!;

        public IList<UserProject> UserProject { get; set; } = null!;

        public User? Manager { get; set; }

        public int? ManagerId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Range(1, 10)]
        public int Priority { get; set; }

        public IList<Issue> Issues { get; set; } = null!;
    }
}
