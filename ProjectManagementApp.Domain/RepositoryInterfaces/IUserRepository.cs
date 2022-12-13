using Microsoft.AspNetCore.Identity;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task Create(User newUser);

        Task Update(User updatedUser);

        User? Get(int id);

        IEnumerable<User> GetAll();

        IEnumerable<Project> GetProjects(int id);

        Task AddToProject(int projectId, int userId);

        Task RemoveFromProject(int projectId, int userId);

        IEnumerable<IdentityRole<int>> GetRoles();

        Task Delete(int id);
    }
}
