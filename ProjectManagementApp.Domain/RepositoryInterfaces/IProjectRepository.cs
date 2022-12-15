using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IProjectRepository
    {
        Task CreateAsync(Project newProject);

        Task UpdateAsync(Project updatedProject);

        Task DeleteAsync(int id);

        Task<Project?> GetByIdAsync(int id);

        Task<IList<Project>> GetAllAsync();

        Task<IList<Project>> GetManagerProjectsAsync(int managerId);
    }
}
