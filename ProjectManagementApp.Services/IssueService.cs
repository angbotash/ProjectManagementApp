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

            if (issue is null)
            {
                throw new KeyNotFoundException($"There is no Issue with Id {newIssue.Id}.");
            }

            await _issueRepository.Create(newIssue);
        }

        public async Task Edit(Issue updatedIssue)
        {
            var issue = _issueRepository.Get(updatedIssue.Id);

            if (issue is null)
            {
                throw new KeyNotFoundException($"There is no Issue with Id {updatedIssue.Id}.");
            }
            
            await _issueRepository.Update(updatedIssue);
        }

        public async Task Delete(int id)
        {
            var issue = _issueRepository.Get(id);

            if (issue is null)
            {
                throw new KeyNotFoundException($"There is no Issue with Id {id}.");
            }
            
            await _issueRepository.Delete(id);
        }

        public Issue? Get(int id)
        {
            return _issueRepository.Get(id);
        }
    }
}
