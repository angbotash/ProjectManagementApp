using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Domain.ServiceInterfaces;

namespace ProjectManagementApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this._employeeRepository = employeeRepository;
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

        public Employee? Get(string email)
        {
            var employee = this._employeeRepository.Get(email);

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
