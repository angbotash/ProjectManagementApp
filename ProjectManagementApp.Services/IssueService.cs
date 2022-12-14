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
            _issueRepository = issueRepository;
        }

        public async Task Create(Issue newIssue)
        {
            var issue = _issueRepository.Get(newIssue.Id);

            if (issue == null)
            {
                await _issueRepository.Create(newIssue);
            }
        }

        public async Task Edit(Issue updatedIssue)
        {
            var issue = _issueRepository.Get(updatedIssue.Id);

            if (issue != null)
            {
                await _issueRepository.Update(updatedIssue);
            }
        }

        public async Task Delete(int id)
        {
            var issue = _issueRepository.Get(id);

            if (issue != null)
            {
                await _issueRepository.Delete(id);
            }
        }

        public Issue? Get(int id)
        {
            var issue = _issueRepository.Get(id);

            return issue;
        }
    }
}
