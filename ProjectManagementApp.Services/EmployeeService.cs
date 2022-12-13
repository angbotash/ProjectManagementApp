using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Domain.ServiceInterfaces;

namespace ProjectManagementApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IProjectRepository _projectRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IProjectRepository projectRepository)
        {
            this._employeeRepository = employeeRepository;
            this._projectRepository = projectRepository;
        }

        public async Task Create(Employee newEmployee)
        {
            var employee = this._employeeRepository.Get(newEmployee.Id);

            if (employee == null)
            {
                await this._employeeRepository.Create(newEmployee);
            }
        }

        public async Task Edit(Employee updatedEmployee)
        {
            var employee = this._employeeRepository.Get(updatedEmployee.Id);

            if (employee != null)
            {
                await this._employeeRepository.Update(updatedEmployee);
            }
        }

        public Employee? Get(int id)
        {
            var employee = this._employeeRepository.Get(id);

            return employee;
        }

        public IEnumerable<Employee> GetAll()
        {
            var employees = this._employeeRepository.GetAll();

            return employees;
        }

        public IEnumerable<Project> GetProjects(int id)
        {
            var projects = this._employeeRepository.GetProjects(id);

            return projects;
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

            await this._employeeRepository.AddToProject(projectId, employeeId);
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

            await this._employeeRepository.RemoveFromProject(projectId, employeeId);
        }

        public async Task Delete(int id)
        {
            var employee = this._employeeRepository.Get(id);

            if (employee != null)
            {
                await this._employeeRepository.Delete(id);
            }
        }
    }
}
