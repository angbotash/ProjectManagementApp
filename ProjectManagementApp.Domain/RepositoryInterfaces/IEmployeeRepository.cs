using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IEmployeeRepository
    {
        Task Create(Employee newEmployee);

        Task Update(int id, string firstName, string lastName, string patronymic, string email);

        Task<Employee?> Get(int id);

        Task<Employee?> Get(string email);

        IEnumerable<Employee> GetAll();

        Task<IEnumerable<Project>> GetProjects(int id);

        Task<bool> IsEmployeeOnProject(int employeeId, int projectId);

        Task Delete(int id);
    }
}
