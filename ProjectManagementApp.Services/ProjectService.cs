using ProjectManagementApp.Domain.Entities;
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

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _projectRepository.GetAllAsync();
        }

        public  async Task<IEnumerable<Project>> GetManagerProjectsAsync(int managerId)
        {
            return await _projectRepository.GetManagerProjectsAsync(managerId);
        }
    }
}
