using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IProjectRepository
    {
        Task Create(Project newProject);

        Task Update(Project updatedProject);

        Task Delete(int id);

        Project? Get(int id);

        IEnumerable<Project> GetAll();

        IEnumerable<Project> GetManagerProjects(int managerId);

        IEnumerable<User> GetUsers(int id);
    }
}
