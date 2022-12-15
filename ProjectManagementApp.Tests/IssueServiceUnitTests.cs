using Moq;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Services;
using Xunit;

namespace ProjectManagementApp.Tests
{
    public class IssueServiceUnitTests
    {
        private readonly IssueService _issueService;
        private readonly Mock<IIssueRepository> _issueRepositoryMock;
        public IssueServiceUnitTests()
        {
            _issueRepositoryMock = new Mock<IIssueRepository>();
            _issueService = new IssueService(_issueRepositoryMock.Object);
        }

        [Fact]
        public async Task Create_Creates_Issue()
        {
            // Arrange
            var newIssue = new Issue
            {
                Name = "New Issue Name",
                AssigneeId = 1,
                ReporterId = 2,
                ProjectId = 3,
                Comment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Status = IssueStatus.ToDo,
                Priority = 1
            };

            _issueRepositoryMock.Setup(m => m.Create(newIssue));

            // Act
            await _issueService.Create(newIssue);

            // Assert
            _issueRepositoryMock.Verify(m => m.Create(It.IsAny<Issue>()), Times.Once);
        }

        [Fact]
        public async Task Delete_Deletes_Issue()
        {
            // Arrange
            var id = 1;

            _issueRepositoryMock.Setup(m => m.GetById(id)).ReturnsAsync(Issues[0]);
            _issueRepositoryMock.Setup(m => m.Delete(id));

            // Act
            await _issueService.Delete(id);

            // Assert
            _issueRepositoryMock.Verify(m => m.GetById(It.IsAny<int>()), Times.Once);
            _issueRepositoryMock.Verify(m => m.Delete(It.IsAny<int>()));
        }

        [Fact]
        public async Task GetById_Returns_Issue()
        {
            // Arrange
            var id = 2;

            _issueRepositoryMock.Setup(m => m.GetById(id)).ReturnsAsync(Issues[1]);

            // Act
            var result = await _issueService.GetById(id);

            // Assert
            Assert.IsType<Issue?>(result);
            Assert.Equal(Issues[1].Id, result.Id);
            _issueRepositoryMock.Verify(m => m.GetById(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public void GetById_Returns_Null_When_Issue_Does_Not_Exist()
        {
            // Arrange
            var id = 42;

            _issueRepositoryMock.Setup(m => m.GetById(id)).ReturnsAsync(Issues.Find(i => i.Id == id));

            // Act
            var result = _issueService.GetById(id);

            // Assert
            Assert.Null(result);
            _issueRepositoryMock.Verify(m => m.GetById(It.IsAny<int>()), Times.Once());
        }

        private static User Assignee { get; } = new User()
        {
            Id = 2,
            FirstName = "Kate",
            LastName = "Osborn",
            Email = "kateosborn@mail.com",
        };

        private static User Reporter { get; } = new User()
        {
            Id = 1,
            FirstName = "Marry",
            LastName = "Smith",
            Email = "marrysmith@mail.com",
        };

        private static Project Project { get; } = new Project()
        {

        };

        private List<Issue> Issues { get; } = new List<Issue>()
        {
            new Issue()
            {
                Id = 1,
                Name = "IssueOneName",
                AssigneeId = Assignee.Id,
                ReporterId = Reporter.Id,
                ProjectId = Project.Id,
                Comment = string.Empty,
                Status = IssueStatus.ToDo,
                Priority = 7
            },
            new Issue()
            {
                Id = 2,
                Name = "IssueTwoName",
                AssigneeId = Assignee.Id,
                ReporterId = Reporter.Id,
                ProjectId = Project.Id,
                Comment = string.Empty,
                Status = IssueStatus.InProgress,
                Priority = 7
            },
            new Issue()
            {
                Id = 3,
                Name = "IssueThreeName",
                AssigneeId = Assignee.Id,
                ReporterId = Reporter.Id,
                ProjectId = Project.Id,
                Comment = string.Empty,
                Status = IssueStatus.Done,
                Priority = 7
            }
        };
    }
}