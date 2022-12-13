using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Domain.ServiceInterfaces;

namespace ProjectManagementApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;

        public UserService(IUserRepository userRepository, IProjectRepository projectRepository)
        {
            this._userRepository = userRepository;
            this._projectRepository = projectRepository;
        }

        public async Task Create(User newUser)
        {
            var user = this._userRepository.Get(newUser.Id);

            if (user == null)
            {
                await this._userRepository.Create(newUser);
            }
        }

        public async Task Edit(User updatedUser)
        {
            var user = this._userRepository.Get(updatedUser.Id);

            if (user != null)
            {
                await this._userRepository.Update(updatedUser);
            }
        }

        public User? Get(int id)
        {
            var user = this._userRepository.Get(id);

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            var users = this._userRepository.GetAll();

            return users;
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

        public async Task Delete(int id)
        {
            var user = this._userRepository.Get(id);

            if (user != null)
            {
                await this._userRepository.Delete(id);
            }
        }
    }
}
