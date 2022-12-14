using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.Infrastructure;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Domain.ServiceInterfaces;

namespace ProjectManagementApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(IUserRepository userRepository, IProjectRepository projectRepository, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._userRepository = userRepository;
            this._projectRepository = projectRepository;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public async Task<OperationResult> Create(User newUser, string password, string role)
        {
            var result = new OperationResult(false);
            var user = await this._userManager.FindByEmailAsync(newUser.Email);

            if (user != null)
            {
                result.AddError("This email is already in use.");

                return result;
            }

            newUser.UserName = newUser.Email;

            var createResult = await this._userManager.CreateAsync(newUser, password);

            if (createResult.Succeeded)
            {
                role = role == string.Empty ? "Employee" : role;

                await this._userManager.AddToRoleAsync(newUser, role);

                return new OperationResult(true);
            }

            foreach (var error in createResult.Errors)
            {
                result.AddError(error.Description);
            }

            return result;
        }

        public async Task<OperationResult> Edit(User updatedUser)
        {
            var result = new OperationResult(false);
            var user = this._userRepository.Get(updatedUser.Id);

            if (user != null)
            {
                await this._userRepository.Update(updatedUser);
            }

            if (user != null)
            {
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Patronymic = updatedUser.Patronymic;
                user.Email = updatedUser.Email;

                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    return new OperationResult(true);
                }

                foreach (var error in updateResult.Errors)
                {
                    result.AddError(error.Description);
                }
            }

            return result;
        }

        public User? Get(int id)
        {
            var user = this._userRepository.Get(id);

            return user;
        }

        public async Task<int?> GetCurrentUserId(ClaimsPrincipal currentUser)
        {
            var user = await this._userManager.GetUserAsync(currentUser);

            if (user != null)
            {
                var id = user.Id;

                return id;
            }

            return null;
        }

        public IEnumerable<User> GetAll()
        {
            var users = this._userRepository.GetAll();

            return users;
        }

        public async Task<IEnumerable<User>> GetManagers()
        {
            var managers = await this._userManager.GetUsersInRoleAsync(Role.Manager.ToString());

            return managers;
        }

        public async Task<IEnumerable<User>> GetEmployees()
        {
            var employees = await this._userManager.GetUsersInRoleAsync(Role.Employee.ToString());

            return employees;
        }

        public IEnumerable<Project> GetProjects(int id)
        {
            var projects = this._userRepository.GetProjects(id);

            return projects;
        }

        public async Task AddToProject(int projectId, int userId)
        {
            if (this._projectRepository.Get(projectId) == null)
            {
                return;
            }

            if (this._userRepository.Get(userId) == null)
            {
                return;
            }

            await this._userRepository.AddToProject(projectId, userId);
        }

        public async Task RemoveFromProject(int projectId, int userId)
        {
            if (this._projectRepository.Get(projectId) == null)
            {
                return;
            }

            if (this._userRepository.Get(userId) == null)
            {
                return;
            }

            await this._userRepository.RemoveFromProject(projectId, userId);
        }

        public IEnumerable<IdentityRole<int>> GetRoles()
        {
            var roles = this._userRepository.GetRoles();

            return roles;
        }

        public async Task Delete(int id)
        {
            var user = this._userRepository.Get(id);

            if (user != null)
            {
                await this._userRepository.Delete(id);
            }
        }

        public async Task<OperationResult> Authenticate(string email, string password)
        {
            var userExists = await this._userManager.FindByEmailAsync(email);
            var result = new OperationResult(false);

            if (userExists != null && await _userManager.CheckPasswordAsync(userExists, password))
            {
                var resultLogin = await _signInManager.PasswordSignInAsync(userExists, password, true, false);

                if (resultLogin.Succeeded)
                {
                    return new OperationResult(true);
                }
            }

            result.AddError("Invalid email or password.");

            return result;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
