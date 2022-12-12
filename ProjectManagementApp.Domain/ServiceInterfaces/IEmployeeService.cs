using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IEmployeeService
    {
        Task Create(Employee newEmployee);

        Task Edit(Employee updatedEmployee);

        Employee? Get(int id);

        Employee? Get(string email);

        IEnumerable<Employee> GetAll();

        IEnumerable<Project> GetProjects(int id);

        Task Delete(int id);
    }
}
