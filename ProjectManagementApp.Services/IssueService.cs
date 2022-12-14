using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.QueryOrder;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Domain.ServiceInterfaces;

namespace ProjectManagementApp.Services
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepository;

        public IssueService(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        public async Task CreateAsync(Issue newIssue)
        {
            await _issueRepository.CreateAsync(newIssue);
        }

        public async Task EditAsync(Issue updatedIssue)
        {
            var issue = await _issueRepository.GetByIdAsync(updatedIssue.Id);

            if (issue is null)
            {
                throw new KeyNotFoundException($"There is no Issue with Id {updatedIssue.Id}.");
            }
            
            await _issueRepository.UpdateAsync(updatedIssue);
        }

        public async Task DeleteAsync(int id)
        {
            var issue = await _issueRepository.GetByIdAsync(id);

            if (issue is null)
            {
                throw new KeyNotFoundException($"There is no Issue with Id {id}.");
            }
            
            await _issueRepository.DeleteAsync(id);
        }

        public async Task<Issue?> GetByIdAsync(int id)
        {
            return await _issueRepository.GetByIdAsync(id);
        }

        public async Task<IList<Issue>> GetReportedIssuesAsync(int userId,
            SortDirection direction = SortDirection.Ascending, string? order = null)
        {
            return await _issueRepository.GetReportedIssuesAsync(userId, direction, order);
        }

        public async Task<IList<Issue>> GetAssignedIssuesAsync(int userId,
            SortDirection direction = SortDirection.Ascending, string? order = null)
        {
            return await _issueRepository.GetAssignedIssuesAsync(userId, direction, order);
        }

        public async Task<IList<Issue>> GetProjectIssuesAsync(int projectId,
            SortDirection direction = SortDirection.Ascending, string? order = null)
        {
            return await _issueRepository.GetProjectIssuesAsync(projectId, direction, order);
        }
    }
}
