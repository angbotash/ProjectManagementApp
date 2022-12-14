using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        [StringLength(30, MinimumLength = 3)]
        [Required]
        public string FirstName { get; set; } = null!;

        [StringLength(30, MinimumLength = 3)]
        [Required]
        public string LastName { get; set; } = null!;

        [StringLength(30, MinimumLength = 3)]
        public string? Patronymic { get; set; }

        public IList<UserProject> UserProject { get; set; } = null!;

        public IList<Issue> AssignedIssues { get; set; } = null!;

        public IList<Issue> ReportedIssues { get; set; } = null!;
    }
}
