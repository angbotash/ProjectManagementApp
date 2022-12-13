using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IProjectRepository
    {
        Task Create(Project newProject);

        Task Update(Project updatedProject);

        Project? Get(int id);

        IEnumerable<Project> GetAll();

        IEnumerable<User> GetUsers(int id);

        Task Delete(int id);

        bool IsUserOnProject(int userId, int projectId);
    }
}
