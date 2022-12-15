using Microsoft.AspNetCore.Identity;
using Moq;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Services;

namespace ProjectManagementApp.Tests
{
    public class ProjectServiceUnitTests
    {
        private readonly ProjectService _projectService;

        public ProjectServiceUnitTests()
        {
            var projectRepositoryMock = new Mock<IProjectRepository>();

            _projectService = new ProjectService(projectRepositoryMock.Object);
        }
    }
}