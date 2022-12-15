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

        public async Task Create(Project newProject)
        {
            await _projectRepository.Create(newProject);
        }

        public async Task Edit(Project updatedProject)
        {
            await _projectRepository.Update(updatedProject);
        }

        public async Task Delete(int id)
        {
            var project = await _projectRepository.GetById(id);

            if (project is null)
            {
                throw new KeyNotFoundException($"There is no Project with Id {id}.");
            }

            await _projectRepository.Delete(id);
        }

        public async Task<Project?> GetById(int id)
        {
            return await _projectRepository.GetById(id);
        }

        public async Task<IEnumerable<Project>> GetAll()
        {
            return await _projectRepository.GetAll();
        }

        public  async Task<IEnumerable<Project>> GetManagerProjects(int managerId)
        {
            return await _projectRepository.GetManagerProjects(managerId);
        }
    }
}
