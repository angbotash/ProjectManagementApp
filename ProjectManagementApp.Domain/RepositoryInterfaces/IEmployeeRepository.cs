using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IEmployeeRepository
    {
        Task Create(Employee newEmployee);
        
        Task Update(Employee updatedEmployee);

        Employee? Get(int id);

        Employee? Get(string email);

        IEnumerable<Employee> GetAll();

        IEnumerable<Project> GetProjects(int id);

        bool IsEmployeeOnProject(int employeeId, int projectId);

        Task Delete(int id);
    }
}
