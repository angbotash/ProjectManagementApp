using Microsoft.AspNetCore.Identity;

namespace ProjectManagementApp.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Patronymic { get; set; }

        public IList<UserProject> UserProjects { get; set; } = null!;

        public IList<Issue> AssignedIssues { get; set; } = null!;

        public IList<Issue> ReportedIssues { get; set; } = null!;
    }
}
