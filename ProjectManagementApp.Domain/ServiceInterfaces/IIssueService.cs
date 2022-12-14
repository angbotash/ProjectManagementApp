using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Domain.ServiceInterfaces
{
    public interface IIssueService
    {
        Task Create(Issue newIssue);

        Task Edit(Issue updatedIssue);

        Task Delete(int id);

        Issue? Get(int id);
    }
}
