using Microsoft.AspNetCore.Identity;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task Create(User newUser);

        Task Update(User updatedUser);

        Task Delete(int id);

        User? Get(int id);

        IEnumerable<User> GetAll();

        IEnumerable<Project> GetProjects(int id);

        IEnumerable<IdentityRole<int>> GetRoles();

        Task AddToProject(int projectId, int userId);

        Task RemoveFromProject(int projectId, int userId);
    }
}
