using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IIssueService
    {
        Task Create(Issue newIssue);

        Task Edit(Issue updatedIssue);

        Task Delete(int id);

        Task<Issue?> GetById(int id);
    }
}
