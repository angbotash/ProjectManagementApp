using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IProjectRepository
    {
        Task Create(Project newProject);

        Task Update(int id, string name, string clientCompanyName, string executorCompanyName, int? managerId, DateTime startDate, DateTime endDate, int priority);

        Task<Project?> Get(int id);

        Task<Project?> Get(string name);

        IEnumerable<Project> GetAll();

        Task<IEnumerable<Employee>> GetEmployees(int id);

        Task Delete(int id);

        //void AddToProject(int projectId, IEnumerable<Employee> employees);

        Task AddToProject(int projectId, int employeeId);

        //void RemoveFromProject(int projectId, IEnumerable<Employee> employees);

        Task RemoveFromProject(int projectId, int employeeId);

        Task<bool> IsEmployeeOnProject(int employeeId, int projectId);
    }
}
