using Microsoft.AspNetCore.Identity;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.Infrastructure;
using System.Security.Claims;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IUserService
    {
        Task<OperationResult> Create(User newUser, string password, string role);

        Task<OperationResult> Edit(User updatedUser);

        Task Delete(int id);

        Task<OperationResult> Authenticate(string email, string password);

        Task Logout();

        User? Get(int id);

        IEnumerable<User> GetAll();

        IEnumerable<Project> GetProjects(int id);

        IEnumerable<IdentityRole<int>> GetRoles();

        Task<User?> GetCurrentUser(ClaimsPrincipal currentUser);

        Task<IEnumerable<User>> GetManagers();

        Task<IEnumerable<User>> GetEmployees();

        Task AddToProject(int projectId, int userId);

        Task RemoveFromProject(int projectId, int userId);
    }
}
