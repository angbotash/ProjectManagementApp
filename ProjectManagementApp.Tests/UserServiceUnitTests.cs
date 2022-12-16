using Microsoft.AspNetCore.Identity;
using Moq;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.Infrastructure;
using ProjectManagementApp.Domain.QueryOrder;
using ProjectManagementApp.Domain.RepositoryInterfaces;
using ProjectManagementApp.Domain.ServiceInterfaces;
using ProjectManagementApp.Services;
using ProjectManagementApp.Tests.IdentityMock;
using Xunit;

namespace ProjectManagementApp.Tests
{
    public class UserServiceUnitTests
    {
        private readonly IUserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly SignInManagerMock _signInManagerMock;

        public UserServiceUnitTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            _signInManagerMock = new SignInManagerMock();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _projectRepositoryMock.Object,
                _userManagerMock.Object,
                _signInManagerMock);
        }

        [Fact]
        public async Task CreateAsync_Creates_User_Returns_OperationResult()
        {
            // Arrange
            var newUser = new User()
            {
                FirstName = "Lorem",
                LastName = "Ipsum",
                Patronymic = "Dolor",
                Email = "loremipsum@mail.com"
            };

            _userManagerMock.Setup(m => m.FindByEmailAsync(newUser.Email)).ReturnsAsync((User?)null);
            _userManagerMock.Setup(m => m.CreateAsync(newUser, It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.AddToRoleAsync(newUser, It.IsAny<string>()));

            // Act
            var result = await _userService.CreateAsync(newUser, It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.Equal(new OperationResult(true), result);
            _userManagerMock.Verify(m => m.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Returns_OperationResult_If_Email_Exists()
        {
            // Arrange
            var newUser = new User()
            {
                FirstName = "Lorem",
                LastName = "Ipsum",
                Patronymic = "Dolor",
                Email = "loremipsum@mail.com"
            };

            _userManagerMock.Setup(m => m.FindByEmailAsync(newUser.Email)).ReturnsAsync(new User());
            _userManagerMock.Setup(m => m.CreateAsync(newUser, It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.AddToRoleAsync(newUser, It.IsAny<string>()));

            // Act
            var result = await _userService.CreateAsync(newUser, It.IsAny<string>(), It.IsAny<string>());
            var expected = new OperationResult(false);
            expected.AddError("This email is already in use.");

            // Assert
            Assert.Equal(expected, result);
            Assert.Equal(expected.Errors[0], result.Errors[0]);
            _userManagerMock.Verify(m => m.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task EditAsync_Edits_User_Returns_OperationResult()
        {
            // Arrange
            var user = new User()
            {
                FirstName = "Lorem",
                LastName = "Ipsum",
                Patronymic = "Dolor",
                Email = "loremipsum@mail.com"
            };

            _userRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.EditAsync(user);

            // Assert
            Assert.Equal(new OperationResult(true), result);
            _userRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task EditAsync_Returns_OperationResult_If_User_Does_Not_Exists()
        {
            // Arrange
            var user = new User()
            {
                Id = 42,
                FirstName = "Lorem",
                LastName = "Ipsum",
                Patronymic = "Dolor",
                Email = "loremipsum@mail.com"
            };

            _userRepositoryMock.Setup(m => m.GetByIdAsync(user.Id)).ReturnsAsync((User?)null);
            _userManagerMock.Setup(m => m.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _userService.EditAsync(user);
            var expected = new OperationResult(false);
            expected.AddError($"There is no User with Id {user.Id}.");

            // Assert
            Assert.Equal(expected, result);
            Assert.Equal(expected.Errors[0], result.Errors[0]);
            _userRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_Deletes_User()
        {
            // Arrange
            _userRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new User());
            _userRepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>()));

            // Act
            await _userService.DeleteAsync(It.IsAny<int>());

            // Assert
            _userRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _userRepositoryMock.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Throws_KeyNotFoundException_When_User_Does_Not_Exists()
        {
            // Arrange
            _userRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User?)null);
            _userRepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>()));

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.DeleteAsync(It.IsAny<int>()));
            _userRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _userRepositoryMock.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task AuthenticateAsync_Authenticates_User_Returns_OperationResult()
        {
            // Arrange
            _userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _userManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);
            var passwordResult = await _signInManagerMock.PasswordSignInAsync(new User(), It.IsAny<string>(), true, false);

            // Act
            var result = await _userService.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.Equal(new OperationResult(true), result);
            _userManagerMock.Verify(m => m.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(m => m.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_Returns_OperationResult_If_User_Does_Not_Exists()
        {
            // Arrange
            _userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            _userManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = await _userService.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>());
            var expected = new OperationResult(false);
            expected.AddError("Invalid email or password.");

            // Assert
            Assert.Equal(expected, result);
            Assert.Equal(expected.Errors[0], result.Errors[0]);
            _userManagerMock.Verify(m => m.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(m => m.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Returns_User()
        {
            // Arrange
            _userRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new User());

            // Act
            var result = await _userService.GetByIdAsync(It.IsAny<int>());

            // Assert
            Assert.IsType<User?>(result);
            _userRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_Returns_Null_When_User_Does_Not_Exist()
        {
            // Arrange
            _userRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetByIdAsync(It.IsAny<int>());

            // Assert
            Assert.Null(result);
            _userRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task GetOrderedListAsync_Returns_Collection_Of_Users()
        {
            // Arrange
            _userRepositoryMock.Setup(m => m.GetOrderedListAsync(
                    SortDirection.Ascending,
                    It.IsAny<string>()))
                .ReturnsAsync(new List<User>());

            // Act
            var result = await _userService.GetOrderedListAsync(
                SortDirection.Ascending,
                It.IsAny<string>());

            // Assert
            Assert.IsType<List<User>>(result);
            _userRepositoryMock.Verify(m => m.GetOrderedListAsync(
                SortDirection.Ascending,
                It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task GetRolesAsync_Returns_Collection_Of_IdentityRoles()
        {
            // Arrange
            _userRepositoryMock.Setup(m => m.GetRolesAsync()).ReturnsAsync(new List<IdentityRole<int>>());

            // Act
            var result = await _userService.GetRolesAsync();

            // Assert
            Assert.IsType<List<IdentityRole<int>>>(result);
            _userRepositoryMock.Verify(m => m.GetRolesAsync(), Times.Once());
        }

        [Fact]
        public async Task AddToProjectAsync_Adds_User_To_Project()
        {
            // Arrange
            _projectRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Project());
            _userRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new User());
            _userRepositoryMock.Setup(m => m.AddToProjectAsync(It.IsAny<int>(), It.IsAny<int>()));

            // Act
            await _userService.AddToProjectAsync(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            _projectRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _userRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _userRepositoryMock.Verify(m => m.AddToProjectAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task AddToProjectAsync_Throws_KeyNotFoundException_When_Project_Does_Not_Exist()
        {
            // Arrange
            _projectRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Project?)null);
            _userRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new User());
            _userRepositoryMock.Setup(m => m.AddToProjectAsync(It.IsAny<int>(), It.IsAny<int>()));

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.AddToProjectAsync(It.IsAny<int>(), It.IsAny<int>()));
            _projectRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _userRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _userRepositoryMock.Verify(m => m.AddToProjectAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task AddToProjectAsync_Throws_KeyNotFoundException_When_User_Does_Not_Exist()
        {
            // Arrange
            _projectRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Project());
            _userRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User?)null);
            _userRepositoryMock.Setup(m => m.AddToProjectAsync(It.IsAny<int>(), It.IsAny<int>()));

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.AddToProjectAsync(It.IsAny<int>(), It.IsAny<int>()));
            _projectRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _userRepositoryMock.Verify(m => m.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _userRepositoryMock.Verify(m => m.AddToProjectAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}