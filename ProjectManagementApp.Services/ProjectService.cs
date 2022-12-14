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
            var project = _projectRepository.Get(id);

            return project;
        }

        public IEnumerable<Project> GetAll()
        {
            var projects = _projectRepository.GetAll();

            return projects;
        }

        public IEnumerable<Project> GetManagerProjects(int managerId)
        {
            var projects = _projectRepository.GetManagerProjects(managerId);

            return projects;
        }

        public IEnumerable<User> GetUsers(int projectId)
        {
            var users = _projectRepository.GetUsers(projectId);

            return users;
        }
    }
}
