using AutoFixture;
using Microsoft.AspNetCore.Identity;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Users;
using Moq;

namespace MicroZoo.IdentityApi.Tests.UnitTests.Services
{
    public class CustomPasswordValidatorTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly CustomPasswordValidator<User> _validator;

        public CustomPasswordValidatorTests()
        {
            _mockUserManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                null!, // options
                null!, // password hasher
                null!, // user validators
                null!, // password validators
                null!, // key normalizer
                null!, // errors
                null!, // services
                null!  // logger
            );

            _validator = new Fixture().Build<CustomPasswordValidator<User>>().Create();
        }

        [Fact]
        public async Task ValidateAsync_NullUser_ReturnsFailed()
        {
            // Arrange    
            var password = "SupperSicretPa$$w0rd";

            // Act
            var result = await _validator.ValidateAsync(_mockUserManager.Object, null!, password);

            // Assert
            Assert.NotEqual(IdentityResult.Success.Succeeded, result.Succeeded);
            Assert.Single(result.Errors);
            Assert.Equal("NullUser", result.Errors.First().Code);
            Assert.Equal("User can't be null.", result.Errors.First().Description);
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        public async Task ValidateAsync_NullOrEmptyPassword_ReturnsFailed(string password)
        {
            // Arrange            
            var user = new Fixture().Build<User>()
                .With(x => x.UserName, "User1")
                .Create();

            // Act
            var result = await _validator.ValidateAsync(_mockUserManager.Object, user, password);

            // Assert
            Assert.NotEqual(IdentityResult.Success.Succeeded, result.Succeeded);
            Assert.Single(result.Errors);
            Assert.Equal("PasswordIsNullOrEmpty", result.Errors.First().Code);
            Assert.Equal("Password can't be null or empty.", result.Errors.First().Description);
        }

        [Fact]
        public async Task ValidateAsync_SameUserAndPassword_ReturnsFailed()
        {
            // Arrange
            var user = new Fixture().Build<User>()
                .With(x => x.UserName, "SupperSicretPa$$w0rd")
                .Create();
            var password = "SupperSicretPa$$w0rd";

            _mockUserManager.Setup(x => x.GetUserNameAsync(It.IsAny<User>()))
                .ReturnsAsync(user.UserName);

            // Act
            var result = await _validator.ValidateAsync(_mockUserManager.Object, user, password);

            // Assert
            Assert.NotEqual(IdentityResult.Success.Succeeded, result.Succeeded);
            Assert.Single(result.Errors);
            Assert.Equal("SameUserPass", result.Errors.First().Code);
            Assert.Equal("Username and Password can't be the same.", 
                result.Errors.First().Description);
        }

        [Fact]
        public async Task ValidateAsync_PasswordContainsPassword_ReturnsFailed()
        {
            // Arrange
            var user = new Fixture().Build<User>()
                .With(x => x.UserName, "User1")
                .Create();
            var password = "SupperSicretPassword1";

            _mockUserManager.Setup(x => x.GetUserNameAsync(It.IsAny<User>()))
                .ReturnsAsync(user.UserName);

            // Act
            var result = await _validator.ValidateAsync(_mockUserManager.Object, user, password);

            // Assert
            Assert.NotEqual(IdentityResult.Success.Succeeded, result.Succeeded);
            Assert.Single(result.Errors);
            Assert.Equal("PasswordContainsPassword", result.Errors.First().Code);
            Assert.Equal("The word \"Password\" is not allowed for the password.",
                result.Errors.First().Description);
        }

        [Fact]
        public async Task ValidateAsync_CorrectData_ReturnsSuccess()
        {
            // Arrange
            var user = new Fixture().Build<User>()
                .With(x => x.UserName, "User1")
                .Create();
            var password = "SupperSicretPa$$w0rd";            
           
            _mockUserManager.Setup(x => x.GetUserNameAsync(It.IsAny<User>()))
                .ReturnsAsync(user.UserName);

            // Act
            var result = await _validator.ValidateAsync(_mockUserManager.Object, user, password);

            // Assert
            Assert.Equal(IdentityResult.Success, result);
        }
    }
}
