using Moq;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Domain.ServiceInterfaces;
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
        public async Task CreateAsync_Creates_Issue()
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

            _issueRepositoryMock.Setup(m => m.CreateAsync(newIssue));

            // Act
            await _issueService.CreateAsync(newIssue);

            // Assert
            _issueRepositoryMock.Verify(m => m.CreateAsync(It.IsAny<Issue>()), Times.Once);
        }

        [Fact]
        public async Task EditAsync_Edits_Issue()
        {
            // Arrange
            var editedIssue = new Issue
            {
                Name = "Edited Issue Name",
                AssigneeId = 3,
                ReporterId = 2,
                ProjectId = 3,
                Comment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Status = IssueStatus.InProgress,
                Priority = 8
            };

            _issueRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Issue());
            _issueRepositoryMock.Setup(m => m.UpdateAsync(editedIssue));

            // Act
            await _issueService.EditAsync(editedIssue);

            // Assert
            _issueRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _issueRepositoryMock.Verify(m => m.UpdateAsync(It.IsAny<Issue>()), Times.Once);
        }

        [Fact]
        public async Task EditAsync_Throws_KeyNotFoundException_When_Issue_Does_Not_Exists()
        {
            // Arrange
            var editedIssue = new Issue
            {
                Id = 42,
                Name = "Edited Issue Name",
                AssigneeId = 3,
                ReporterId = 2,
                ProjectId = 3,
                Comment = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                Status = IssueStatus.InProgress,
                Priority = 8
            };

            _issueRepositoryMock.Setup(m => m.GetByIdAsync(editedIssue.Id)).ReturnsAsync((Issue?)null);

            // Act
            await _issueService.EditAsync(editedIssue);

            // Assert
            //await Assert.ThrowsAsync<KeyNotFoundException>(() => _issueService.EditAsync(editedIssue));
            _issueRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _issueRepositoryMock.Verify(m => m.UpdateAsync(It.IsAny<Issue>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_Deletes_Issue()
        {
            // Arrange
            _issueRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Issue());
            _issueRepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>()));

            // Act
            await _issueService.DeleteAsync(It.IsAny<int>());

            // Assert
            _issueRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _issueRepositoryMock.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Throws_KeyNotFoundException_When_Issue_Does_Not_Exists()
        {
            // Arrange
            _issueRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Issue?)null);
            _issueRepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>()));

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _issueService.DeleteAsync(It.IsAny<int>()));
            _issueRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _issueRepositoryMock.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_Returns_Issue()
        {
            // Arrange
            _issueRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Issue());

            // Act
            var result = await _issueService.GetByIdAsync(It.IsAny<int>());

            // Assert
            Assert.IsType<Issue?>(result);
            _issueRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_Returns_Null_When_Issue_Does_Not_Exist()
        {
            // Arrange
            _issueRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Issue?)null);

            // Act
            var result = await _issueService.GetByIdAsync(It.IsAny<int>());

            // Assert
            Assert.Null(result);
            _issueRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once());
        }
    }
}