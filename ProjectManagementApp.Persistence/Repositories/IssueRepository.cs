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
            var task = _dbContext.Issues.FirstOrDefault(i => i.Id == updatedTask.Id);

            if (task != null)
            {
                task.Name = updatedTask.Name;
                task.AssigneeId = updatedTask.AssigneeId;
                task.Comment = updatedTask.Comment;
                task.Status = updatedTask.Status;
                task.Priority = updatedTask.Priority;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var issue = _dbContext.Issues.FirstOrDefault(i => i.Id == id);

            if (issue != null)
            {
                _dbContext.Issues.Remove(issue);
                await _dbContext.SaveChangesAsync();
            }
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
