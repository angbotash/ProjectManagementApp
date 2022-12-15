using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.QueryOrder;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IIssueService
    {
        Task CreateAsync(Issue newIssue);

        Task EditAsync(Issue updatedIssue);

        Task DeleteAsync(int id);

        Task<Issue?> GetByIdAsync(int id);

        Task<IEnumerable<Issue>> GetReportedIssuesAsync(int userId, SortDirection direction = SortDirection.Ascending,
            string? order = null, string? filter = null);

        Task<IEnumerable<Issue>> GetAssignedIssuesAsync(int userId, SortDirection direction = SortDirection.Ascending,
            string? order = null, string? filter = null);

        Task<IEnumerable<Issue>> GetProjectIssuesAsync(int projectId, SortDirection direction = SortDirection.Ascending,
            string? order = null, string? filter = null);
    }
}
