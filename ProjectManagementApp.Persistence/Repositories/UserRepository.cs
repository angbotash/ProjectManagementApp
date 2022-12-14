using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.QueryOrder;
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

        public async Task DeleteAsync(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user is null)
            {
                throw new KeyNotFoundException($"There is no User with Id {id}.");
            }

            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await _dbContext.Users
                .Include(u => u.AssignedIssues)
                    .ThenInclude(i => i.Reporter)
                .Include(u => u.AssignedIssues)
                    .ThenInclude(i => i.Project)
                .Include(u => u.ReportedIssues)
                    .ThenInclude(i => i.Assignee)
                .Include(u => u.ReportedIssues)
                    .ThenInclude(i=> i.Project)
                .Include(u => u.UserProjects)
                    .ThenInclude(up => up.Project)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<IList<User>> GetOrderedListAsync(SortDirection direction = SortDirection.Ascending, string? order = null)
        {
            return await GetOrderedListQuery(direction, order).ToListAsync();
        }

        public async Task<IList<IdentityRole<int>>> GetRolesAsync()
        {
            return await _dbContext.Roles.ToListAsync();
        }

        public async Task AddToProjectAsync(int projectId, int userId)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                return;
            }

            var userProject = await _dbContext.UserProject
                .FirstOrDefaultAsync(p => p.ProjectId == projectId && p.UserId == userId);

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

        public async Task RemoveFromProjectAsync(int projectId, int userId)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                return;
            }

            var userProject = await _dbContext.UserProject
                .FirstOrDefaultAsync(p => p.ProjectId == projectId && p.UserId == userId);

            if (userProject != null)
            {
                _dbContext.UserProject.Remove(userProject);
                await _dbContext.SaveChangesAsync();
            }
        }

        private IQueryable<User> GetOrderedListQuery(SortDirection direction = SortDirection.Ascending,
            string? order = null)
        {
            var query = _dbContext.Users
                .Include(u => u.AssignedIssues)
                    .ThenInclude(i => i.Reporter)
                .Include(u => u.AssignedIssues)
                    .ThenInclude(i => i.Project)
                .Include(u => u.ReportedIssues)
                    .ThenInclude(i => i.Assignee)
                .Include(u => u.ReportedIssues)
                    .ThenInclude(i => i.Project)
                .Include(u => u.UserProjects)
                    .ThenInclude(up => up.Project)
                .AsQueryable();

            if (order != null)
            {
                query = query.OrderBy($"{order} {direction}");
            }

            return query;
        }
    }
}
