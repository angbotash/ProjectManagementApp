using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IIssueService
    {
        Task CreateAsync(Issue newIssue);

        Task EditAsync(Issue updatedIssue);

        Task DeleteAsync(int id);

        Task<Issue?> GetByIdAsync(int id);
    }
}
