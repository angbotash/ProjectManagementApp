using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Domain.ServiceInterfaces;

namespace ProjectManagementApp.Services
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepository;

        public IssueService(IIssueRepository issueRepository)
        {
            this._issueRepository = issueRepository;
        }

        public async Task Create(Issue newIssue)
        {
            var issue = this._issueRepository.Get(newIssue.Id);

            if (issue == null)
            {
                await this._issueRepository.Create(newIssue);
            }
        }

        public async Task Edit(Issue updatedIssue)
        {
            var issue = this._issueRepository.Get(updatedIssue.Id);

            if (issue != null)
            {
                await this._issueRepository.Update(updatedIssue);
            }
        }

        public Issue? Get(int id)
        {
            var issue = this._issueRepository.Get(id);

            return issue;
        }

        public IEnumerable<Issue>? GetAllIssues()
        {
            var issues = this._issueRepository.GetAllIssues();

            return issues;
        }

        public async Task Delete(int id)
        {
            var issue = this._issueRepository.Get(id);

            if (issue != null)
            {
                await this._issueRepository.Delete(id);
            }
        }
    }
}
