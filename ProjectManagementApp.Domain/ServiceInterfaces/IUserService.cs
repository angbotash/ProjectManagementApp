using Microsoft.AspNetCore.Identity;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.Infrastructure;
using System.Security.Claims;
using ProjectManagementApp.Domain.QueryOrder;

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

        Task<IList<User>> GetOrderedListAsync(SortDirection direction = SortDirection.Ascending, string? order = null);

        Task<IList<IdentityRole<int>>> GetRolesAsync();

        Task<User?> GetCurrentUserAsync(ClaimsPrincipal currentUser);

        Task<IList<User>> GetManagersAsync();

        Task<IList<User>> GetEmployeesAsync();

        Task AddToProjectAsync(int projectId, int userId);

        Task RemoveFromProjectAsync(int projectId, int userId);
    }
}
