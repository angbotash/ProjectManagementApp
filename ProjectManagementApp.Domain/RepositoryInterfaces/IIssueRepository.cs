using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IIssueRepository
    {
        Task Create(Issue newTask);

        Task Update(Issue updatedTask);

        Task Delete(int id);

        Issue? Get(int id);
    }
}
