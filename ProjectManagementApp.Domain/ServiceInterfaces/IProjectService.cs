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

        Task<IList<Project>> GetOrderedListAsync(SortDirection direction = SortDirection.Ascending, string? order = null);
        
        Task<IList<Project>> GetManagerProjectsAsync(int managerId,
            SortDirection direction = SortDirection.Ascending, string? order = null);

        Task<IList<Project>> GetEmployeeProjectsAsync(int employeeId,
            SortDirection direction = SortDirection.Ascending, string? order = null);
    }
}
