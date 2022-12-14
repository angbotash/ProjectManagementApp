using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IProjectService
    {
        Task Create(Project newProject);

        Task Edit(Project updatedProject);

        Task Delete(int id);

        Project? Get(int id);

        IEnumerable<Project> GetAll();

        IEnumerable<Project> GetManagerProjects(int managerId);

        IEnumerable<User> GetUsers(int projectId);
    }
}
