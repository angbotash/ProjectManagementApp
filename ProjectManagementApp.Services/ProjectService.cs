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
            this._projectRepository = projectRepository;
        }

        public async Task Create(Project newProject)
        {
            var project = this._projectRepository.Get(newProject.Id);

            if (project == null)
            {
                await this._projectRepository.Create(newProject);
            }
        }

        public async Task Edit(Project updatedProject)
        {
            var project = this._projectRepository.Get(updatedProject.Id);

            if (project != null)
            {
                await this._projectRepository.Update(updatedProject);
            }
        }

        public Project? Get(int id)
        {
            var project = this._projectRepository.Get(id);

            return project;
        }

        public IEnumerable<Project> GetAll()
        {
            var projects = this._projectRepository.GetAll();

            return projects;
        }

        public IEnumerable<Project> GetManagerProjects(int managerId)
        {
            var projects = this._projectRepository.GetManagerProjects(managerId);

            return projects;
        }

        public IEnumerable<User> GetUsers(int projectId)
        {
            var users = this._projectRepository.GetUsers(projectId);

            return users;
        }

        public async Task Delete(int id)
        {
            var project = this._projectRepository.Get(id);

            if (project != null)
            {
                await this._projectRepository.Delete(id);
            }
        }
    }
}
