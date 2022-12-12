using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IProjectRepository
    {
        Task Create(Project newProject);

        Task Update(Project updatedProject);

        Project? Get(int id);

        Project? Get(string name);

        IEnumerable<Project> GetAll();

        IEnumerable<Employee> GetEmployees(int id);

        Task Delete(int id);

        //void AddToProject(int projectId, IEnumerable<Employee> employees);

        Task AddToProject(int projectId, int employeeId);

        //void RemoveFromProject(int projectId, IEnumerable<Employee> employees);

        Task RemoveFromProject(int projectId, int employeeId);

        bool IsEmployeeOnProject(int employeeId, int projectId);
    }
}
