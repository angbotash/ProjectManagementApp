using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IProjectService
    {
        Task Create(Project newProject);

        Task Edit(Project updatedProject);

        Project? Get(int id);

        IEnumerable<Project> GetAll();

        IEnumerable<Employee> GetEmployees(int id);

        bool IsOnProject(int projectId, int employeeId);

        Task Delete(int id);
    }
}
