using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Domain.Entities;
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

        public async Task Create(Issue newTask)
        {
            await _dbContext.Issues.AddAsync(newTask);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Issue updatedTask)
        {
            var issue = _dbContext.Issues.FirstOrDefault(i => i.Id == updatedTask.Id);

            if (issue is null)
            {
                throw new KeyNotFoundException($"There is no Issue with Id {updatedTask.Id}.");
            }

            issue.Name = updatedTask.Name;
            issue.AssigneeId = updatedTask.AssigneeId;
            issue.Comment = updatedTask.Comment;
            issue.Status = updatedTask.Status;
            issue.Priority = updatedTask.Priority;

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var issue = _dbContext.Issues.FirstOrDefault(i => i.Id == id);

            if (issue is null)
            {
                throw new KeyNotFoundException($"There is no Issue with Id {id}.");
            }

            _dbContext.Issues.Remove(issue);
            await _dbContext.SaveChangesAsync();
        }

        public Issue? Get(int id)
        {
            var issue = _dbContext.Issues
                .Include(i => i.Project)
                .Include(i => i.Assignee)
                .Include(i => i.Reporter)
                .FirstOrDefault(i => i.Id == id);

            return issue;
        }
    }
}
