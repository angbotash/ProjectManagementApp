using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApp.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Patronymic { get; set; }

        public IList<UserProject> UserProject { get; set; } = null!;

        public IList<Issue> AssignedIssues { get; set; } = null!;

        public IList<Issue> ReportedIssues { get; set; } = null!;
    }
}
