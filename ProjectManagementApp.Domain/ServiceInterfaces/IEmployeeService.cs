using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IEmployeeService
    {
        Task Create(Employee newEmployee);

        Task Edit(Employee updatedEmployee);

        Task<Employee?> Get(int id);

        Task<Employee?> Get(string email);

        IEnumerable<Employee> GetAll();

        Task<IEnumerable<Project>> GetProjects(int id);

        Task Delete(int id);
    }
}
