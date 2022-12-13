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
            await this._dbContext.Issues.AddAsync(newTask);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task Update(Issue updatedTask)
        {
            var task = this._dbContext.Issues.FirstOrDefault(p => p.Id == updatedTask.Id);

            if (task != null)
            {
                task.Name = updatedTask.Name;
                task.AssigneeId = updatedTask.AssigneeId;
                task.Comment = updatedTask.Comment;
                task.Status = updatedTask.Status;
                task.Priority = updatedTask.Priority;

                await this._dbContext.SaveChangesAsync();
            }
        }

        public Issue? Get(int id)
        {
            var issue = this._dbContext.Issues.Include(x => x.Project)
                .Include(x => x.Assignee)
                .Include(x => x.Reporter)
                .FirstOrDefault(i => i.Id == id);

            return issue;
        }

        public Issue? Get(string name)
        {
            var issue = this._dbContext.Issues.Include(x => x.Project)
                .Include(x => x.Assignee)
                .Include(x => x.Reporter)
                .FirstOrDefault(i => i.Name == name);

            return issue;
        }

        public IEnumerable<Issue>? GetAllIssues()
        {
            var issues = this._dbContext.Issues.Include(x => x.Project)
                .Include(x => x.Assignee)
                .Include(x => x.Reporter);

            return issues;
        }

        public async Task Delete(int id)
        {
            var issue = this._dbContext.Issues.FirstOrDefault(i => i.Id == id);

            if (issue != null)
            {
                this._dbContext.Issues.Remove(issue);
                await this._dbContext.SaveChangesAsync();
            }
        }
    }
}
