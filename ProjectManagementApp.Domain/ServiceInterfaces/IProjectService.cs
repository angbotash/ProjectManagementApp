using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IProjectService
    {
        Task CreateAsync(Project newProject);

        Task EditAsync(Project updatedProject);

        Task DeleteAsync(int id);

        Task<Project?> GetByIdAsync(int id);

        Task<IEnumerable<Project>> GetAllAsync();

        Task<IEnumerable<Project>> GetManagerProjectsAsync(int managerId);
    }
}
