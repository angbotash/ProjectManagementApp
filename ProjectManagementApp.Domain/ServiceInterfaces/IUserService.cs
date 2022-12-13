using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IUserService
    {
        Task Create(User newUser);

        Task Edit(User updatedUser);

        User? Get(int id);

        IEnumerable<User> GetAll();

        IEnumerable<Project> GetProjects(int id);

        Task AddToProject(int projectId, int userId);

        Task RemoveFromProject(int projectId, int userId);

        Task Delete(int id);
    }
}
