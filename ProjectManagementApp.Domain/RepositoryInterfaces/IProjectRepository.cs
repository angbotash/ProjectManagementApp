using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IProjectRepository
    {
        Task Create(Project newProject);

        Task Update(Project updatedProject);

        Project? Get(int id);

        IEnumerable<Project> GetAll();

        IEnumerable<Employee> GetEmployees(int id);

        Task Delete(int id);

        Task AddToProject(int projectId, int employeeId);

        Task RemoveFromProject(int projectId, int employeeId);

        bool IsEmployeeOnProject(int employeeId, int projectId);
    }
}
