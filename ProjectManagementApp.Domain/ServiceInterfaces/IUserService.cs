using Microsoft.AspNetCore.Identity;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.Infrastructure;
using System.Security.Claims;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IUserService
    {
        Task<OperationResult> CreateAsync(User newUser, string password, string role);

        Task<OperationResult> EditAsync(User updatedUser);

        Task DeleteAsync(int id);

        Task<OperationResult> AuthenticateAsync(string email, string password);

        Task LogoutAsync();

        Task<User?> GetByIdAsync(int id);

        Task<IEnumerable<User>> GetAllAsync();

        Task<IEnumerable<IdentityRole<int>>> GetRolesAsync();

        Task<User?> GetCurrentUserAsync(ClaimsPrincipal currentUser);

        Task<IEnumerable<User>> GetManagersAsync();

        Task<IEnumerable<User>> GetEmployeesAsync();

        Task AddToProjectAsync(int projectId, int userId);

        Task RemoveFromProjectAsync(int projectId, int userId);
    }
}
