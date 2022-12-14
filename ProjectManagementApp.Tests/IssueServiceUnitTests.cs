using Moq;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Services;

namespace ProjectManagementApp.Tests
{
    public class IssueServiceUnitTests
    {
        private readonly IssueService _issueService;

        public IssueServiceUnitTests()
        {
            var issueRepositoryMock = new Mock<IIssueRepository>();

            _issueService = new IssueService(issueRepositoryMock.Object);
        }
    }
}
