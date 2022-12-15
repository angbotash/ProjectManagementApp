using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.QueryOrder;

namespace ProjectManagementApp.Domain.RepositoryInterfaces
{
    public interface IIssueRepository
    {
        Task CreateAsync(Issue newIssue);

        Task UpdateAsync(Issue updatedIssue);

        Task DeleteAsync(int id);

        Task<Issue?> GetByIdAsync(int id);

        Task<IList<Issue>> GetReportedIssuesAsync(int userId, SortDirection direction = SortDirection.Ascending,
            string? order = null, string? filter = null);

        Task<IList<Issue>> GetAssignedIssuesAsync(int userId, SortDirection direction = SortDirection.Ascending,
            string? order = null, string? filter = null);

        Task<IList<Issue>> GetProjectIssuesAsync(int projectId, SortDirection direction = SortDirection.Ascending,
            string? order = null, string? filter = null);
    }
}
