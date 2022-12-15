﻿using Microsoft.AspNetCore.Identity;
using Moq;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Domain.ServiceInterfaces;
using ProjectManagementApp.Services;
using Xunit;

namespace ProjectManagementApp.Tests
{
    public class ProjectServiceUnitTests
    {
        private readonly ProjectService _projectService;
        private readonly Mock<IProjectRepository> _projectRepositoryMock;

        public ProjectServiceUnitTests()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();

            _projectService = new ProjectService(_projectRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateAsync_Creates_Project()
        {
            // Arrange
            var newProject = new Project
            {
                Name = "New Project Name",
                ClientCompanyName = "Client",
                ExecutorCompanyName = "Executor",
                StartDate = new DateTime(2022, 12, 12),
                EndDate = new DateTime(2023, 9, 15),
                Priority = 1
            };

            _projectRepositoryMock.Setup(m => m.CreateAsync(newProject));

            // Act
            await _projectService.CreateAsync(newProject);

            // Assert
            _projectRepositoryMock.Verify(m => m.CreateAsync(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public async Task EditAsync_Edits_Project()
        {
            // Arrange
            var editedProject = new Project()
            {
                Name = "Edited Project Name",
                ClientCompanyName = "Client Edited",
                ExecutorCompanyName = "Executor Edited",
                StartDate = new DateTime(2022, 12, 12),
                EndDate = new DateTime(2023, 11, 11),
                Priority = 4
            };
            
            _projectRepositoryMock.Setup(m => m.UpdateAsync(editedProject));

            // Act
            await _projectService.EditAsync(editedProject);

            // Assert
            _projectRepositoryMock.Verify(m => m.UpdateAsync(It.IsAny<Project>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Deletes_Project()
        {
            // Arrange
            _projectRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Project());
            _projectRepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>()));

            // Act
            await _projectService.DeleteAsync(It.IsAny<int>());

            // Assert
            _projectRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _projectRepositoryMock.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Throws_KeyNotFoundException_When_Project_Does_Not_Exists()
        {
            // Arrange
            _projectRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Project?)null);
            _projectRepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>()));

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _projectService.DeleteAsync(It.IsAny<int>()));
            _projectRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _projectRepositoryMock.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_Returns_Project()
        {
            // Arrange
            _projectRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Project());

            // Act
            var result = await _projectService.GetByIdAsync(It.IsAny<int>());

            // Assert
            Assert.IsType<Project?>(result);
            _projectRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_Returns_Null_When_Project_Does_Not_Exist()
        {
            // Arrange
            _projectRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Project?)null);

            // Act
            var result = await _projectService.GetByIdAsync(It.IsAny<int>());

            // Assert
            Assert.Null(result);
            _projectRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task GetAllAsync_Returns_Collection_Of_Projects()
        {
            // Arrange
            _projectRepositoryMock.Setup(m => m.GetAllAsync()).ReturnsAsync(new List<Project>());

            // Act
            var result = await _projectService.GetAllAsync();

            // Assert
            Assert.IsType<List<Project>>(result);
            _projectRepositoryMock.Verify(m => m.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task GetManagerProjectsAsync_Returns__Projects_Of_Manager()
        {
            // Arrange
            _projectRepositoryMock.Setup(m => m.GetManagerProjectsAsync(It.IsAny<int>())).ReturnsAsync(new List<Project>());

            // Act
            var result = await _projectService.GetManagerProjectsAsync(It.IsAny<int>());

            // Assert
            Assert.IsType<List<Project>>(result);
            _projectRepositoryMock.Verify(m => m.GetManagerProjectsAsync(It.IsAny<int>()), Times.Once());
        }
    }
}