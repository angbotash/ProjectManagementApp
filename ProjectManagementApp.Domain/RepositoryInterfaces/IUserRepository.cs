using Microsoft.AspNetCore.Identity;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task DeleteAsync(int id);

        Task<User?> GetByIdAsync(int id);

        Task<IList<IdentityRole<int>>> GetRolesAsync();

        Task AddToProjectAsync(int projectId, int userId);

        Task RemoveFromProjectAsync(int projectId, int userId);
    }
}
