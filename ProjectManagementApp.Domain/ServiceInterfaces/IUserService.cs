using Microsoft.AspNetCore.Identity;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.Infrastructure;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IUserService
    {
        Task<OperationResult> Create(User newUser, string password, string role);

        Task<OperationResult> Edit(User updatedUser);

        User? Get(int id);

        IEnumerable<User> GetAll();

        IEnumerable<Project> GetProjects(int id);

        Task AddToProject(int projectId, int userId);

        Task RemoveFromProject(int projectId, int userId);

        IEnumerable<IdentityRole<int>> GetRoles();

        Task Delete(int id);

        Task<OperationResult> Authenticate(string email, string password);

        Task Logout();
    }
}
