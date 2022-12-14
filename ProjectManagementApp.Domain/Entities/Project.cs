using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 3)]
        [Required]
        public string Name { get; set; } = null!;

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string ClientCompanyName { get; set; } = null!;

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string ExecutorCompanyName { get; set; } = null!;

        public IList<UserProject> UserProject { get; set; } = null!;

        public User? Manager { get; set; }

        public int? ManagerId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(1, 10)]
        [Required]
        public int Priority { get; set; }

        public IList<Issue> Issues { get; set; } = null!;
    }
}
