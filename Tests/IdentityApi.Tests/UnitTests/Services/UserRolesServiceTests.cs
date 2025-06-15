using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MicroZoo.IdentityApi.Repositories;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Roles;
using MicroZoo.Infrastructure.Models.Users;
using Moq;
using Org.BouncyCastle.Crypto;

namespace MicroZoo.IdentityApi.Tests.UnitTests.Services
{
    public class UserRolesServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUsersRepository> _usersRepository;
        private readonly Mock<IUserRolesRepository> _userRolesRepository;
        private readonly Mock<IRolesRepository> _rolesRepository;
        private readonly Mock<ILogger<UserRolesService>> _logger;
        private readonly UserRolesService _userRolesService;

        public UserRolesServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _usersRepository = _fixture.Freeze<Mock<IUsersRepository>>();
            _userRolesRepository = _fixture.Freeze<Mock<IUserRolesRepository>>();
            _rolesRepository = _fixture.Freeze<Mock<IRolesRepository>>();
            _logger = _fixture.Freeze<Mock<ILogger<UserRolesService>>>();

            _userRolesService = _fixture.Create<UserRolesService>();
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abcd")]
        [InlineData("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxxx")]
        public async Task GetUserWithRolesAsync_NotGuid_ReturnsUserIdIsNotGuid(string userId)
        {
            // Arrange            
            var expectedMessage = "User Id is not Guid";

            // Act
            var result = await _userRolesService.GetUserWithRolesAsync(userId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task GetUserWithRolesAsync_UserIsNotExist_ReturnsUserDoesNotExist()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var expectedMessage = $"User with Id {userId} does not exist";

            _usersRepository.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _userRolesService.GetUserWithRolesAsync(userId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task GetUserWithRolesAsync_NullRoles_ReturnsInnerServerError()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var selecctedUser = _fixture.Build<User>().Create();
            var expectedMessage = "Inner server error";            

            _usersRepository.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(selecctedUser);
            _userRolesRepository.Setup(x => x.GetRolesOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync((List<Role>)null!);

            // Act
            var result = await _userRolesService.GetUserWithRolesAsync(userId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task GetUserWithRolesAsync_EmptyRoles_ReturnsUserWithEmptyRoles()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var selecctedUser = _fixture.Build<User>().Create();            

            _usersRepository.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(selecctedUser);
            _userRolesRepository.Setup(x => x.GetRolesOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Role>());

            // Act
            var result = await _userRolesService.GetUserWithRolesAsync(userId);

            // Assert
            Assert.NotNull(result.UserWithRoles);
            Assert.NotNull(result.UserWithRoles.Roles);
            Assert.Empty(result.UserWithRoles.Roles!);
        }

        [Fact]
        public async Task GetUserWithRolesAsync_CorrectRequest_ReturnsUserWithRoles()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var selecctedUser = _fixture.Build<User>().Create();
            var roles = _fixture.Build<Role>()
                .OmitAutoProperties()
                .CreateMany(5)                
                .ToList();

            _usersRepository.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(selecctedUser);
            _userRolesRepository.Setup(x => x.GetRolesOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(roles);

            // Act
            var result = await _userRolesService.GetUserWithRolesAsync(userId);

            // Assert
            Assert.NotNull(result.UserWithRoles);
            Assert.NotEmpty(result.UserWithRoles.Roles!);
            Assert.Equal(5, result.UserWithRoles.Roles!.Count);
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abcd")]
        [InlineData("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxxx")]
        public async Task UpdateUserWithRolesAsync_NotGuid_ReturnsUserIdIsNotGuid(string userId)
        {
            // Arrange            
            var expectedMessage = "User Id is not Guid";

            // Act
            var result = await _userRolesService.UpdateUserWithRolesAsync(userId, null!);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateUserWithRolesAsync_RoleIdsNotGuid_ReturnsNotAllRoleIdsAreGuid()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var roleIds = new List<string> { "Not Guid 1", "Nor Goud 2" };
            var expectedMessage = "Not all role Ids are Guid";

            // Act
            var result = await _userRolesService.UpdateUserWithRolesAsync(userId, roleIds);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateUserWithRolesAsync_InvalidListOfRolesIds_ReturnsInvalidListOfRolesIds()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var roleIds = _fixture
                .CreateMany<string>(5)
                .ToList();
            var expectedMessage = "Invalid list of roles Ids";

            _rolesRepository.Setup(x => x.CheckEntriesIsExistInDatabase(It.IsAny<List<string>>()))
                .Returns(false);

            // Act
            var result = await _userRolesService.UpdateUserWithRolesAsync(userId, roleIds);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateUserWithRolesAsync_UserNotExist_ReturnsUserDoesNotExixst()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var roleIds = _fixture
                .CreateMany<string>(5)
                .ToList();
            var expectedMessage = $"User with Id {userId} does not exist";

            _rolesRepository.Setup(x => x.CheckEntriesIsExistInDatabase(It.IsAny<List<string>>()))
                .Returns(true);
            _usersRepository.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _userRolesService.UpdateUserWithRolesAsync(userId, roleIds);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateUserWithRolesAsync_UnsuccessfullyDeletedUserRoles_ReturnsInnerServerError()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var roleIds = _fixture
                .CreateMany<string>(5)
                .ToList();
            var selectedUser = _fixture.Build<User>().Create();
            var expectedMessage = "Inner server error";

            _rolesRepository.Setup(x => x.CheckEntriesIsExistInDatabase(It.IsAny<List<string>>()))
                .Returns(true);
            _usersRepository.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(selectedUser);
            _userRolesRepository.Setup(x => x.DeleteUserRolesByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _userRolesService.UpdateUserWithRolesAsync(userId, roleIds);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateUserWithRolesAsync_UnsuccessfullyAddedUserRoles_ReturnsInnerServerError()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var roleIds = _fixture
                .CreateMany<string>(5)
                .ToList();
            var selectedUser = _fixture.Build<User>().Create();
            var expectedMessage = "Inner server error";

            _rolesRepository.Setup(x => x.CheckEntriesIsExistInDatabase(It.IsAny<List<string>>()))
                .Returns(true);
            _usersRepository.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(selectedUser);
            _userRolesRepository.Setup(x => x.DeleteUserRolesByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _userRolesRepository.Setup(x => 
                x.AddUserRolesAsync(It.IsAny<List<IdentityUserRole<string>>>()))
                .ReturnsAsync(false);

            // Act
            var result = await _userRolesService.UpdateUserWithRolesAsync(userId, roleIds);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateUserWithRolesAsync_CorrectRequest_ReturnsUserWithRolesAsync()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var roleIds = _fixture
                .CreateMany<string>(5)
                .ToList();
            var selectedUser = _fixture.Build<User>().Create();
            var roles = _fixture.Build<Role>()
                .OmitAutoProperties()
                .CreateMany(5)
                .ToList();

            _rolesRepository.Setup(x => x.CheckEntriesIsExistInDatabase(It.IsAny<List<string>>()))
                .Returns(true);
            _usersRepository.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(selectedUser);
            _userRolesRepository.Setup(x => x.DeleteUserRolesByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _userRolesRepository.Setup(x =>
                x.AddUserRolesAsync(It.IsAny<List<IdentityUserRole<string>>>()))
                .ReturnsAsync(true);
            _userRolesRepository.Setup(x => x.GetRolesOfUserAsync(It.IsAny<string>()))
                .ReturnsAsync(roles);

            var userIdsString = string.Join(", ", roleIds);

            // Act
            var result = await _userRolesService.UpdateUserWithRolesAsync(userId, roleIds);

            // Assert
            Assert.NotNull(result.UserWithRoles);
            Assert.NotEmpty(result.UserWithRoles.Roles!);
            Assert.Equal(5, result.UserWithRoles.Roles!.Count);
            _logger.VerifyLog(LogLevel.Information,
                $"New roles have been set for user with Id {userId}. " + 
                $"New role Ids: {userIdsString}", Times.Once());
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abcd")]
        [InlineData("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxxx")]
        public async Task DeleteUserRolesByUserIdAsync_NotGuid_ReturnsUserIdIsNotGuid(string userId)
        {
            // Arrange            
            //var expectedMessage = "User Id is not Guid";

            // Act
            var result = await _userRolesService.DeleteUserRolesByUserIdAsync(userId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteUserRolesByUserIdAsync_UserRolesUnsuccessfullyDeleted_ReturnsFalse()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();

            _userRolesRepository.Setup(x =>
                x.DeleteUserRolesByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _userRolesService.DeleteUserRolesByUserIdAsync(userId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteUserRolesByUserIdAsync_CorrectRequest_ReturnsTrue()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();

            _userRolesRepository.Setup(x =>
                x.DeleteUserRolesByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _userRolesService.DeleteUserRolesByUserIdAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abcd")]
        [InlineData("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxxx")]
        public async Task DeleteUserRolesByRoleIdAsync_NotGuid_ReturnsUserIdIsNotGuid(string roleId)
        {
            // Arrange            
            //var expectedMessage = "User Id is not Guid";

            // Act
            var result = await _userRolesService.DeleteUserRolesByRoleIdAsync(roleId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteUserRolesByRoleIdAsync_UserRolesUnsuccessfullyDeleted_ReturnsFalse()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();

            _userRolesRepository.Setup(x =>
                x.DeleteUserRolesByRoleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _userRolesService.DeleteUserRolesByRoleIdAsync(roleId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteUserRolesByRoleIdAsync_CorrectRequest_ReturnsTrue()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();

            _userRolesRepository.Setup(x =>
                x.DeleteUserRolesByRoleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _userRolesService.DeleteUserRolesByRoleIdAsync(roleId);

            // Assert
            Assert.True(result);
        }
    }
}
