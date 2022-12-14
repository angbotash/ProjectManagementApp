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
            var project = _projectRepository.Get(newProject.Id);

            if (project is null)
            {
                throw new KeyNotFoundException($"There is no Project with Id {newProject.Id}.");
            }

            await _projectRepository.Create(newProject);
        }

        public async Task Edit(Project updatedProject)
        {
            await _projectRepository.Update(updatedProject);
        }

        public async Task Delete(int id)
        {
            var project = _projectRepository.Get(id);

            if (project is null)
            {
                throw new KeyNotFoundException($"There is no Project with Id {id}.");
            }

            await _projectRepository.Delete(id);
        }

        public Project? Get(int id)
        {
            return _projectRepository.Get(id);
        }

        public IEnumerable<Project> GetAll()
        {
            return _projectRepository.GetAll();
        }

        public IEnumerable<Project> GetManagerProjects(int managerId)
        {
            return _projectRepository.GetManagerProjects(managerId);
        }

        public IEnumerable<User> GetUsers(int projectId)
        {
            return _projectRepository.GetUsers(projectId);
        }
    }
}
