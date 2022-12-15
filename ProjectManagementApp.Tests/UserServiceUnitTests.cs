using Microsoft.AspNetCore.Identity;
using Moq;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Services;

namespace ProjectManagementApp.Tests
{
    public class UserServiceUnitTests
    {
        private readonly UserService _userService;

        public UserServiceUnitTests()
        {
            var uesrRepositoryMock = new Mock<IUserRepository>();
            var projectRepositoryMock = new Mock<IProjectRepository>();
            var userManagerMock = new Mock<UserManager<User>>();
            var signInManager = new Mock<SignInManager<User>>();

            _userService = new UserService(
                uesrRepositoryMock.Object,
                projectRepositoryMock.Object,
                userManagerMock.Object,
                signInManager.Object);
        }
    }
}