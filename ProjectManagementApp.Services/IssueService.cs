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
            await _issueRepository.Create(newIssue);
        }

        public async Task Edit(Issue updatedIssue)
        {
            var issue = _issueRepository.GetById(updatedIssue.Id);

            if (issue is null)
            {
                throw new KeyNotFoundException($"There is no Issue with Id {updatedIssue.Id}.");
            }
            
            await _issueRepository.Update(updatedIssue);
        }

        public async Task Delete(int id)
        {
            var issue = _issueRepository.GetById(id);

            if (issue is null)
            {
                throw new KeyNotFoundException($"There is no Issue with Id {id}.");
            }
            
            await _issueRepository.Delete(id);
        }

        public async Task<Issue?> GetById(int id)
        {
            return await _issueRepository.GetById(id);
        }
    }
}
