using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IProjectService
    {
        Task Create(Project newProject);

        Task Edit(Project updatedProject);

        Project? Get(int id);

        IEnumerable<Project> GetAll();

        IEnumerable<Project> GetManagerProjects(int managerId);

        IEnumerable<User> GetUsers(int projectId);

        Task Delete(int id);
    }
}
