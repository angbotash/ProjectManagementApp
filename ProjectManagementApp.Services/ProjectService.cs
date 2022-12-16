using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.QueryOrder;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Domain.ServiceInterfaces;

namespace ProjectManagementApp.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task CreateAsync(Project newProject)
        {
            await _projectRepository.CreateAsync(newProject);
        }

        public async Task EditAsync(Project updatedProject)
        {
            await _projectRepository.UpdateAsync(updatedProject);
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);

            if (project is null)
            {
                throw new KeyNotFoundException($"There is no Project with Id {id}.");
            }

            await _projectRepository.DeleteAsync(id);
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _projectRepository.GetByIdAsync(id);
        }

        public async Task<IList<Project>> GetOrderedListAsync(SortDirection direction = SortDirection.Ascending, string? order = null)
        {
            return await _projectRepository.GetOrderedListAsync(direction, order);
        }

        public async Task<IList<Project>> GetManagerProjectsAsync(int managerId,
            SortDirection direction = SortDirection.Ascending, string? order = null)
        {
            return await _projectRepository.GetManagerProjectsAsync(managerId, direction, order);
        }

        public async Task<IList<Project>> GetEmployeeProjectsAsync(int employeeId,
            SortDirection direction = SortDirection.Ascending, string? order = null)
        {
            return await _projectRepository.GetEmployeeProjectsAsync(employeeId, direction, order);
        }
    }
}
