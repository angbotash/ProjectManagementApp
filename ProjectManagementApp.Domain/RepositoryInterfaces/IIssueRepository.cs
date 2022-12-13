using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IIssueRepository
    {
        Task Create(Issue newTask);

        Task Update(Issue updatedTask);

        Issue? Get(int id);

        IEnumerable<Issue>? GetAllIssues();

        Task Delete(int id);
    }
}
