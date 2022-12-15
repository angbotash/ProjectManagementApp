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

        public async Task CreateAsync(Issue newIssue)
        {
            await _issueRepository.CreateAsync(newIssue);
        }

        public async Task EditAsync(Issue updatedIssue)
        {
            var issue = _issueRepository.GetByIdAsync(updatedIssue.Id);

            if (issue is null)
            {
                throw new KeyNotFoundException($"There is no Issue with Id {updatedIssue.Id}.");
            }
            
            await _issueRepository.UpdateAsync(updatedIssue);
        }

        public async Task DeleteAsync(int id)
        {
            var issue = _issueRepository.GetByIdAsync(id);

            if (issue is null)
            {
                throw new KeyNotFoundException($"There is no Issue with Id {id}.");
            }
            
            await _issueRepository.DeleteAsync(id);
        }

        public async Task<Issue?> GetByIdAsync(int id)
        {
            var a = await _issueRepository.GetByIdAsync(id);
            return a;
        }
    }
}
