using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IIssueRepository
    {
        Task Create(Issue newIssue);

        Task Update(Issue updatedIssue);

        Task Delete(int id);

        Task<Issue?> GetById(int id);
    }
}
