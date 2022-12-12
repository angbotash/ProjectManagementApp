using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var project = await this._projectRepository.Get(newProject.Id);

            if (project == null)
            {
                await this._projectRepository.Create(newProject);
            }
        }

        public async Task Edit(Project updatedProject)
        {
            var project = await this._projectRepository.Get(updatedProject.Id);

            if (project != null)
            {
                await this._projectRepository.Update(updatedProject.Id,
                                                     updatedProject.Name,
                                                     updatedProject.ClientCompanyName,
                                                     updatedProject.ExecutorCompanyName,
                                                     updatedProject.ManagerId ,
                                                     updatedProject.StartDate,
                                                     updatedProject.EndDate,
                                                     updatedProject.Priority);
            }
        }

        public async Task<Project?> Get(int id)
        {
            var project = await this._projectRepository.Get(id);

            return project;
        }

        public IEnumerable<Project> GetAll()
        {
            var projects = this._projectRepository.GetAll();

            return projects;
        }

        public async Task<IEnumerable<Employee>> GetEmployees(int id)
        {
            var employees = await this._projectRepository.GetEmployees(id);

            return employees;
        }

        public async Task AddToProject(int projectId, int employeeId)
        {
            if (await this._projectRepository.Get(projectId) == null)
            {
                return;
            }

            if (await this._employeeRepository.Get(employeeId) == null)
            {
                return;
            }

            await this._projectRepository.AddToProject(projectId, employeeId);
        }

        public async Task RemoveFromProject(int projectId, int employeeId)
        {
            if (await this._projectRepository.Get(projectId) == null)
            {
                return;
            }

            if (await this._employeeRepository.Get(employeeId) == null)
            {
                return;
            }

            await this._projectRepository.RemoveFromProject(projectId, employeeId);
        }

        public async Task<bool> IsOnProject(int projectId, int employeeId)
        {
            var result = await this._projectRepository.IsEmployeeOnProject(projectId, employeeId);

            return result;
        }

        public async Task Delete(int id)
        {
            var project = await this._projectRepository.Get(id);

            if (project != null)
            {
                await this._projectRepository.Delete(id);
            }
        }
    }
}
