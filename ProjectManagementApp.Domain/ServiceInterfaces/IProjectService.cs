using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IProjectService
    {
        Task Create(Project newProject);

        Task Edit(Project updatedProject);

        Task<Project?> Get(int id);

        IEnumerable<Project> GetAll();

        Task<IEnumerable<Employee>> GetEmployees(int id);

        Task AddToProject(int projectId, int employeeId);

        Task RemoveFromProject(int projectId, int employeeId);

        Task<bool> IsOnProject(int projectId, int employeeId);

        Task Delete(int id);
    }
}
