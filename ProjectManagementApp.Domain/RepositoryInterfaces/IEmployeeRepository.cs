using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IEmployeeRepository
    {
        Task Create(Employee newEmployee);
        
        Task Update(Employee updatedEmployee);

        Employee? Get(int id);

        IEnumerable<Employee> GetAll();

        IEnumerable<Project> GetProjects(int id);

        Task AddToProject(int projectId, int employeeId);

        Task RemoveFromProject(int projectId, int employeeId);

        bool IsEmployeeOnProject(int employeeId, int projectId);

        Task Delete(int id);
    }
}
