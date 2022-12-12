using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Domain.ServiceInterfaces;

namespace ProjectManagementApp.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ProjectService(IProjectRepository projectRepository, IEmployeeRepository employeeRepository)
        {
            this._projectRepository = projectRepository;
            this._employeeRepository = employeeRepository;
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

        public IEnumerable<Employee> GetEmployees(int id)
        {
            var employees = this._projectRepository.GetEmployees(id);

            return employees;
        }

        public async Task AddToProject(int projectId, int employeeId)
        {
            if (this._projectRepository.Get(projectId) == null)
            {
                return;
            }

            if (this._employeeRepository.Get(employeeId) == null)
            {
                return;
            }

            await this._projectRepository.AddToProject(projectId, employeeId);
        }

        public async Task RemoveFromProject(int projectId, int employeeId)
        {
            if (this._projectRepository.Get(projectId) == null)
            {
                return;
            }

            if (this._employeeRepository.Get(employeeId) == null)
            {
                return;
            }

            await this._projectRepository.RemoveFromProject(projectId, employeeId);
        }

        public bool IsOnProject(int projectId, int employeeId)
        {
            var result = this._projectRepository.IsEmployeeOnProject(projectId, employeeId);

            return result;
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
