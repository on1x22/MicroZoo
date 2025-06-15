using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.Logging;
using MicroZoo.IdentityApi.Models.DTO;
using MicroZoo.IdentityApi.Repositories;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Users;
using Moq;

namespace MicroZoo.IdentityApi.Tests.UnitTests.Services
{
    public class UsersServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUsersRepository> _usersRepository;
        private readonly Mock<IUserRolesService> _userRolesService;
        private readonly Mock<ILogger<UsersService>> _logger;
        private readonly UsersService _usersService;

        public UsersServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _usersRepository = _fixture.Freeze<Mock<IUsersRepository>>();
            _userRolesService = _fixture.Freeze<Mock<IUserRolesService>>();
            _logger = _fixture.Freeze<Mock<ILogger<UsersService>>>();

            _usersService = _fixture.Create<UsersService>();
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsListOfUsers()
        {
            // Arrange            
            var users = _fixture.Build<User>()
                .OmitAutoProperties()
                .CreateMany(5)
                .ToList();            

            _usersRepository.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _usersService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result.Users);
            Assert.NotEmpty(result.Users);
            Assert.Equal(5, result.Users.Count);
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abcd")]
        [InlineData("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxxx")]
        public async Task GetUserAsync_NotGuid_ReturnsUserIdIsNotGuid(string userId)
        {
            // Arrange            
            var expectedMessage = "User Id is not Guid";

            // Act
            var result = await _usersService.GetUserAsync(userId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task GetUserAsync_UserNotExist_ReturnsUserDoesNotExixst()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var expectedMessage = $"User with Id {userId} does not exist";

            _usersRepository.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _usersService.GetUserAsync(userId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task GetUserAsync_CorrectRequest_ReturnsUser()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var selectedUser = _fixture.Build<User>().Create();

            _usersRepository.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(selectedUser);

            // Act
            var result = await _usersService.GetUserAsync(userId);

            // Assert
            Assert.NotNull(result.User);
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abcd")]
        [InlineData("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxxx")]
        public async Task UpdateUserAsync_NotGuid_ReturnsUserIdIsNotGuid(string userId)
        {
            // Arrange            
            var expectedMessage = "User Id is not Guid";

            // Act
            var result = await _usersService.UpdateUserAsync(userId, null!);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateUserAsync_userForUpdateDtoIsNull_ReturnsInvalidData()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var expectedMessage = "Invalid data for update";

            // Act
            var result = await _usersService.UpdateUserAsync(userId, null!);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateUserAsync_UpdateUserIsNull_ReturnsUserDoesNotExist()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var userForUpdateDto = _fixture.Build<UserForUpdateDto>().Create();
            var expectedMessage = $"User with Id {userId} does not exist";
            
            _usersRepository.Setup(x => 
                x.UpdateUserAsync(It.IsAny<string>(), It.IsAny<UserForUpdateDto>()))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _usersService.UpdateUserAsync(userId, userForUpdateDto);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateUserAsync_CorrectRequest_ReturnsUpdatedUser()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var userForUpdateDto = _fixture.Build<UserForUpdateDto>().Create();
            var updatedUser = _fixture.Build<User>().Create();

            _usersRepository.Setup(x =>
                x.UpdateUserAsync(It.IsAny<string>(), It.IsAny<UserForUpdateDto>()))
                .ReturnsAsync(updatedUser);

            // Act
            var result = await _usersService.UpdateUserAsync(userId, userForUpdateDto);

            // Assert
            Assert.NotNull(result.User);
            _logger.VerifyLog(LogLevel.Information,
                $"Information about user with Id {userId} has been " +
                $"updated: {userForUpdateDto}", Times.Once());
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abcd")]
        [InlineData("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxxx")]
        public async Task SoftDeleteUserAsync_NotGuid_ReturnsUserIdIsNotGuid(string userId)
        {
            // Arrange            
            var expectedMessage = "User Id is not Guid";

            // Act
            var result = await _usersService.SoftDeleteUserAsync(userId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task SoftDeleteUserAsync_UpdateUserIsNull_ReturnsUserDoesNotExist()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var expectedMessage = $"User with Id {userId} does not exist";

            _usersRepository.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _usersService.SoftDeleteUserAsync(userId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task SoftDeleteUserAsync_UnsuccessfullyDeletedUserRoles_ReturnsInnerServerError()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var selectedUser = _fixture.Build<User>().Create();
            var expectedMessage = "Inner server error";

            _usersRepository.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(selectedUser);
            _userRolesService.Setup(x => x.DeleteUserRolesByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _usersService.SoftDeleteUserAsync(userId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task SoftDeleteUserAsync_CorrectRequest_ReturnsDeletedUser()
        {
            // Arrange            
            var userId = Guid.NewGuid().ToString();
            var selectedUser = _fixture.Build<User>().Create();

            _usersRepository.Setup(x => x.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(selectedUser);
            _userRolesService.Setup(x => x.DeleteUserRolesByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _usersRepository.Setup(x => x.SoftDeleteUserAsync(It.IsAny<User>()))
                .ReturnsAsync(selectedUser);

            // Act
            var result = await _usersService.SoftDeleteUserAsync(userId);

            // Assert
            Assert.NotNull(result.User);
            _logger.VerifyLog(LogLevel.Information,
                $"User with Id {userId} has been marked as \"Deleted\"", Times.Once());
        }
    }
}
