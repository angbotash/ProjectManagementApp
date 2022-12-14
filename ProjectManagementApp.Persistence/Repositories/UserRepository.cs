using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;

namespace ProjectManagementApp.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _dbContext;

        public UserRepository(ApplicationContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task Create(User newUser)
        {
            await this._dbContext.Users.AddAsync(newUser);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task Update(User updatedUser)
        {
            var user = this._dbContext.Users.FirstOrDefault(e => e.Id == updatedUser.Id);

            if (user != null)
            {
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Patronymic = updatedUser.Patronymic;
                user.Email = updatedUser.Email;

                await _dbContext.SaveChangesAsync();
            }
        }

        public User? Get(int id)
        {
            var user = this._dbContext.Users
                .Include(x => x.AssignedIssues)
                    .ThenInclude(x => x.Reporter)
                .Include(x => x.AssignedIssues)
                    .ThenInclude(x => x.Project)
                .Include(x => x.ReportedIssues)
                    .ThenInclude(x => x.Assignee)
                .Include(x => x.ReportedIssues)
                    .ThenInclude(x => x.Project)
                .FirstOrDefault(e => e.Id == id);

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            var users = this._dbContext.Users;

            return users;
        }

        public IEnumerable<Project> GetProjects(int id)
        {
            var projects = this._dbContext.UserProject
                .Where(p => p.UserId == id)
                .ToList();
            var result = new List<Project>();

            foreach (var project in projects)
            {
                var tempProject = this._dbContext.Projects
                    .Include(x => x.Manager)
                    .Include(x => x.Issues)
                        .ThenInclude(x => x.Assignee)
                    .Include(x => x.Issues)
                        .ThenInclude(x => x.Reporter)
                    .FirstOrDefault(p => p.Id == project.ProjectId);

                if (tempProject != null)
                {
                    result.Add(tempProject);
                }
            }

            return result;
        }

        public async Task AddToProject(int projectId, int userId)
        {
            var project = this._dbContext.Projects.FirstOrDefault(p => p.Id == projectId);

            if (project == null)
            {
                return;
            }

            var userProject =
                this._dbContext.UserProject.FirstOrDefault(p =>
                    p.ProjectId == projectId && p.UserId == userId);

            if (userProject is null)
            {
                var newUserProject = new UserProject()
                    { UserId = userId, ProjectId = projectId };

                await this._dbContext.UserProject.AddAsync(newUserProject);
                await this._dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveFromProject(int projectId, int userId)
        {
            var project = this._dbContext.Projects.FirstOrDefault(p => p.Id == projectId);

            if (project == null)
            {
                return;
            }

            var userProject =
                this._dbContext.UserProject.FirstOrDefault(p =>
                    p.ProjectId == projectId && p.UserId == userId);

            if (userProject != null)
            {
                this._dbContext.UserProject.Remove(userProject);
                await this._dbContext.SaveChangesAsync();
            }
        }

        public IEnumerable<IdentityRole<int>> GetRoles()
        {
            var roles = this._dbContext.Roles;

            return roles;
        }

        public async Task Delete(int id)
        {
            var user = this._dbContext.Users.FirstOrDefault(e => e.Id == id);

            if (user != null)
            {
                this._dbContext.Remove(user);
                await this._dbContext.SaveChangesAsync();
            }
        }
    }
}
