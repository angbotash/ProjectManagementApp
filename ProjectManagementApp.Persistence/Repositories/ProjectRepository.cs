using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.QueryOrder;
using ProjectManagementApp.Domain.RepositoryInterfaces;

namespace ProjectManagementApp.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationContext _dbContext;

        public ProjectRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Project newProject)
        {
            await _dbContext.Projects.AddAsync(newProject);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project updatedProject)
        {
            this._dbContext.Projects.Update(updatedProject);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);

            if (project is null)
            {
                throw new KeyNotFoundException($"There is no Project with Id {id}.");
            }

            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _dbContext.Projects
                .Include(p => p.Manager)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Assignee)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Reporter)
                .Include(p => p.UserProjects)
                    .ThenInclude(up => up.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IList<Project>> GetOrderedListAsync(SortDirection direction = SortDirection.Ascending, string? order = null)
        {
            return await GetOrderedListQuery(direction, order).ToListAsync();
        }

        public async Task<IList<Project>> GetManagerProjectsAsync(int managerId,
        SortDirection direction = SortDirection.Ascending, string? order = null)
        {
            return await GetOrderedListQuery(direction, order)
                .Where(x => x.ManagerId == managerId)
                .ToListAsync();
        }

        public async Task<IList<Project>> GetEmployeeProjectsAsync(int employeeId,
            SortDirection direction = SortDirection.Ascending, string? order = null)
        {
            return await GetOrderedListQuery(direction, order)
                .Where(x => x.UserProjects.Any(up => up.UserId == employeeId))
                .ToListAsync();
        }

        private IQueryable<Project> GetOrderedListQuery(SortDirection direction = SortDirection.Ascending, string? order = null)
        {
            var query = _dbContext.Projects
                .Include(p => p.Manager)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Assignee)
                .Include(p => p.Issues)
                    .ThenInclude(i => i.Reporter)
                .AsQueryable();

            if (order != null)
            {
                query = query.OrderBy($"{order} {direction}");
            }

            return query;
        }
    }
}
