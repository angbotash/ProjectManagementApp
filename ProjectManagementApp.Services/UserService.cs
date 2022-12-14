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
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<OperationResult> Create(User newUser, string password, string role)
        {
            var result = new OperationResult(false);
            var user = await _userManager.FindByEmailAsync(newUser.Email);

            if (user != null)
            {
                result.AddError("This email is already in use.");

                return result;
            }

            newUser.UserName = newUser.Email;

            var createResult = await _userManager.CreateAsync(newUser, password);

            if (createResult.Succeeded)
            {
                role = role == string.Empty ? "Employee" : role;

                await _userManager.AddToRoleAsync(newUser, role);

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
            var user = _userRepository.Get(updatedUser.Id);

            if (user is null)
            {
                throw new KeyNotFoundException($"There is no User with Id {updatedUser.Id}.");
            }

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

            return result;
        }

        public async Task Delete(int id)
        {
            var user = _userRepository.Get(id);

            if (user is null)
            {
                throw new KeyNotFoundException($"There is no User with Id {id}.");
            }

            await _userRepository.Delete(id);
        }

        public async Task<OperationResult> Authenticate(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            var result = new OperationResult(false);

            if (existingUser != null && await _userManager.CheckPasswordAsync(existingUser, password))
            {
                var resultLogin = await _signInManager.PasswordSignInAsync(existingUser, password, true, false);

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

        public User? Get(int id)
        {
            var user = _userRepository.Get(id);

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            var users = _userManager.Users;

            return users;
        }

        public IEnumerable<Project> GetProjects(int id)
        {
            var projects = _userRepository.GetProjects(id);

            return projects;
        }

        public IEnumerable<IdentityRole<int>> GetRoles()
        {
            var roles = _userRepository.GetRoles();

            return roles;
        }

        public async Task<User?> GetCurrentUser(ClaimsPrincipal currentUser)
        {
            var user = await _userManager.GetUserAsync(currentUser);

            return user;
        }

        public async Task<IEnumerable<User>> GetManagers()
        {
            var managers = await _userManager.GetUsersInRoleAsync(Role.Manager.ToString());

            return managers;
        }

        public async Task<IEnumerable<User>> GetEmployees()
        {
            var employees = await _userManager.GetUsersInRoleAsync(Role.Employee.ToString());

            return employees;
        }

        public async Task AddToProject(int projectId, int userId)
        {
            if (_projectRepository.Get(projectId) == null)
            {
                throw new KeyNotFoundException($"There is no Project with Id {projectId}.");
            }

            if (_userRepository.Get(userId) == null)
            {
                throw new KeyNotFoundException($"There is no User with Id {userId}.");
            }

            await _userRepository.AddToProject(projectId, userId);
        }

        public async Task RemoveFromProject(int projectId, int userId)
        {
            if (_projectRepository.Get(projectId) == null)
            {
                throw new KeyNotFoundException($"There is no Project with Id {projectId}.");
            }

            if (_userRepository.Get(userId) == null)
            {
                throw new KeyNotFoundException($"There is no User with Id {userId}.");
            }

            await _userRepository.RemoveFromProject(projectId, userId);
        }
    }
}
