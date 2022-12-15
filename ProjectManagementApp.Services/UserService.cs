using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<OperationResult> CreateAsync(User newUser, string password, string role)
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
                role = role == string.Empty ? Role.Employee.ToString() : role;

                await _userManager.AddToRoleAsync(newUser, role);

                return new OperationResult(true);
            }

            foreach (var error in createResult.Errors)
            {
                result.AddError(error.Description);
            }

            return result;
        }

        public async Task<OperationResult> EditAsync(User updatedUser)
        {
            var result = new OperationResult(false);
            var user = await _userRepository.GetByIdAsync(updatedUser.Id);

            if (user is null)
            {
                result.AddError($"There is no User with Id {updatedUser.Id}.");

                return result;
            }

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Patronymic = updatedUser.Patronymic;
            user.Email = updatedUser.Email;
            user.UserName = updatedUser.Email;

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

        public async Task DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
            {
                throw new KeyNotFoundException($"There is no User with Id {id}.");
            }

            await _userRepository.DeleteAsync(id);
        }

        public async Task<OperationResult> AuthenticateAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            var result = new OperationResult(false);
            var correctPassword = await _userManager.CheckPasswordAsync(existingUser, password);

            if (existingUser != null && correctPassword)
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

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IEnumerable<IdentityRole<int>>> GetRolesAsync()
        {
            return await _userRepository.GetRolesAsync();
        }

        public async Task<User?> GetCurrentUserAsync(ClaimsPrincipal currentUser)
        {
            return await _userManager.GetUserAsync(currentUser);
        }

        public async Task<IEnumerable<User>> GetManagersAsync()
        {
            return await _userManager.GetUsersInRoleAsync(Role.Manager.ToString());
        }

        public async Task<IEnumerable<User>> GetEmployeesAsync()
        {
            return await _userManager.GetUsersInRoleAsync(Role.Employee.ToString());
        }

        public async Task AddToProjectAsync(int projectId, int userId)
        {
            if (await _projectRepository.GetByIdAsync(projectId) == null)
            {
                throw new KeyNotFoundException($"There is no Project with Id {projectId}.");
            }

            if (await _userRepository.GetByIdAsync(userId) == null)
            {
                throw new KeyNotFoundException($"There is no User with Id {userId}.");
            }

            await _userRepository.AddToProjectAsync(projectId, userId);
        }

        public async Task RemoveFromProjectAsync(int projectId, int userId)
        {
            if (await _projectRepository.GetByIdAsync(projectId) == null)
            {
                throw new KeyNotFoundException($"There is no Project with Id {projectId}.");
            }

            if (await _userRepository.GetByIdAsync(userId) == null)
            {
                throw new KeyNotFoundException($"There is no User with Id {userId}.");
            }

            await _userRepository.RemoveFromProjectAsync(projectId, userId);
        }
    }
}
