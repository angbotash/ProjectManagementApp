using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IIssueRepository
    {
        Task CreateAsync(Issue newIssue);

        Task UpdateAsync(Issue updatedIssue);

        Task DeleteAsync(int id);

        Task<Issue?> GetByIdAsync(int id);
    }
}
