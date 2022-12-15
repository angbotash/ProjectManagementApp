using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.QueryOrder;
using ProjectManagementApp.Domain.RepositoryInterfaces;

namespace ProjectManagementApp.Persistence.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private readonly ApplicationContext _dbContext;

        public IssueRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Issue newIssue)
        {
            await _dbContext.Issues.AddAsync(newIssue);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Issue updatedIssue)
        {
            var issue = await _dbContext.Issues.FirstOrDefaultAsync(i => i.Id == updatedIssue.Id);

            if (issue is null)
            {
                throw new KeyNotFoundException($"There is no Issue with Id {updatedIssue.Id}.");
            }

            issue.Name = updatedIssue.Name;
            issue.AssigneeId = updatedIssue.AssigneeId;
            issue.Comment = updatedIssue.Comment;
            issue.Status = updatedIssue.Status;
            issue.Priority = updatedIssue.Priority;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var issue = await _dbContext.Issues.FirstOrDefaultAsync(i => i.Id == id);

            if (issue is null)
            {
                throw new KeyNotFoundException($"There is no Issue with Id {id}.");
            }

            _dbContext.Issues.Remove(issue);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Issue?> GetByIdAsync(int id)
        {
            return await _dbContext.Issues
                .Include(i => i.Project)
                .Include(i => i.Assignee)
                .Include(i => i.Reporter)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IList<Issue>> GetReportedIssuesAsync(int userId, SortDirection direction = SortDirection.Ascending, string? order = null,
            string? filter = null)
        {
            var query = _dbContext.Issues
                .Include(i => i.Project)
                .Include(i => i.Assignee)
                .Include(i => i.Reporter)
                .Where(i => i.ReporterId == userId)
                .AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (order != null)
            {
                query = query.OrderBy($"{order} {direction}");
            }

            return await query.ToListAsync();
        }

        public async Task<IList<Issue>> GetAssignedIssuesAsync(int userId, SortDirection direction = SortDirection.Ascending, string? order = null,
            string? filter = null)
        {
            var query = _dbContext.Issues
                .Include(i => i.Project)
                .Include(i => i.Assignee)
                .Include(i => i.Reporter)
                .Where(i => i.AssigneeId == userId)
                .AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (order != null)
            {
                query = query.OrderBy($"{order} {direction}");
            }

            return await query.ToListAsync();
        }

        public async Task<IList<Issue>> GetProjectIssuesAsync(int projectId, SortDirection direction = SortDirection.Ascending, string? order = null,
            string? filter = null)
        {
            var query = _dbContext.Issues
                .Include(i => i.Project)
                .Include(i => i.Assignee)
                .Include(i => i.Reporter)
                .Where(i => i.Project.Id == projectId)
                .AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (order != null)
            {
                query = query.OrderBy($"{order} {direction}");
            }

            return await query.ToListAsync();
        }
    }
}
