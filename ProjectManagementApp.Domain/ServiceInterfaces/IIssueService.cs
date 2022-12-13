using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IIssueService
    {
        Task Create(Issue newIssue);

        Task Edit(Issue updatedIssue);

        Issue? Get(int id);

        IEnumerable<Issue>? GetAllIssues();

        Task Delete(int id);
    }
}
