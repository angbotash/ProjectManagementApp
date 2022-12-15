using Microsoft.AspNetCore.Identity;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task Delete(int id);

        Task<User?> GetById(int id);

        Task<IList<IdentityRole<int>>> GetRoles();

        Task AddToProject(int projectId, int userId);

        Task RemoveFromProject(int projectId, int userId);
    }
}
