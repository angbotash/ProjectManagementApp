using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.QueryOrder;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IProjectService
    {
        Task CreateAsync(Project newProject);

        Task EditAsync(Project updatedProject);

        Task DeleteAsync(int id);

        Task<Project?> GetByIdAsync(int id);

        Task<IEnumerable<Project>> GetOrderedListAsync(SortDirection direction = SortDirection.Ascending,
            string? order = null, string? filter = null);

        Task<IEnumerable<Project>> GetManagerProjectsAsync(int managerId);
    }
}
