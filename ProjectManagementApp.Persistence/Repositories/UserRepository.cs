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
            _dbContext = dbContext;
        }

        public async Task Create(User newUser)
        {
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user is null)
            {
                throw new KeyNotFoundException($"There is no User with Id {id}.");
            }

            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public User? Get(int id)
        {
            var user = _dbContext.Users
                .Include(u => u.AssignedIssues)
                    .ThenInclude(i => i.Reporter)
                .Include(u => u.AssignedIssues)
                    .ThenInclude(i => i.Project)
                .Include(u => u.ReportedIssues)
                    .ThenInclude(i => i.Assignee)
                .Include(u => u.ReportedIssues)
                    .ThenInclude(i=> i.Project)
                .FirstOrDefault(u => u.Id == id);

            return user;
        }

        public IEnumerable<Project> GetProjects(int id)
        {
            var projects = _dbContext.UserProject
                .Where(up => up.UserId == id)
                .ToList();
            var result = new List<Project>();

            foreach (var project in projects)
            {
                var tempProject = _dbContext.Projects
                    .Include(p => p.Manager)
                    .Include(p => p.Issues)
                        .ThenInclude(i => i.Assignee)
                    .Include(p => p.Issues)
                        .ThenInclude(i => i.Reporter)
                    .FirstOrDefault(p => p.Id == project.ProjectId);

                if (tempProject != null)
                {
                    result.Add(tempProject);
                }
            }

            return result;
        }

        public IEnumerable<IdentityRole<int>> GetRoles()
        {
            return _dbContext.Roles;
        }

        public async Task AddToProject(int projectId, int userId)
        {
            var project = _dbContext.Projects.FirstOrDefault(p => p.Id == projectId);

            if (project == null)
            {
                return;
            }

            var userProject = _dbContext.UserProject
                .FirstOrDefault(p => p.ProjectId == projectId && p.UserId == userId);

            if (userProject is null)
            {
                var newUserProject = new UserProject()
                {
                    UserId = userId,
                    ProjectId = projectId
                };

                await _dbContext.UserProject.AddAsync(newUserProject);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveFromProject(int projectId, int userId)
        {
            var project = _dbContext.Projects.FirstOrDefault(p => p.Id == projectId);

            if (project == null)
            {
                return;
            }

            var userProject = _dbContext.UserProject
                .FirstOrDefault(p => p.ProjectId == projectId && p.UserId == userId);

            if (userProject != null)
            {
                _dbContext.UserProject.Remove(userProject);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
