using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using MicroZoo.EmailService;
using MicroZoo.IdentityApi.Controllers;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.IdentityApi.Models.DTO;
using MicroZoo.Infrastructure.Models.Users;
using MimeKit;
using Moq;
using System.Web;

namespace MicroZoo.IdentityApi.Tests.UnitTests.Controllers
{
    public class AccountsControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<IJwtHandler> _mockJwtHandler;
        private readonly Mock<ILogger<AccountsController>> _mockLogger;
        private readonly AccountsController _controller;

        public AccountsControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

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

            _mockEmailSender = _fixture.Freeze<Mock<IEmailSender>>();
            _mockJwtHandler = _fixture.Freeze<Mock<IJwtHandler>>();
            _mockLogger = _fixture.Freeze<Mock<ILogger<AccountsController>>>();

            _fixture.Inject(_mockUserManager.Object);
            _fixture.Customize<BindingInfo>(x => x.OmitAutoProperties());
            _controller = _fixture.Create<AccountsController>();
        }

        [Fact]
        public async Task RegisterUser_NullInput_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.RegisterUser(null!);

            // Assert
            Assert.IsType<BadRequestResult>(result);
            _mockLogger.VerifyLog(LogLevel.Warning, 
                "Entered invalid user data for registration",
                Times.Once());
        }

        [Fact]
        public async Task RegisterUser_InvalidEmail_ReturnsBadRequestWithErrors()
        {
            // Arrange
            var invalidUser = _fixture.Build<UserForRegistrationDto>()
                .With(x => x.Email, "invalid-email")
                .Create();

            // Act
            var result = await _controller.RegisterUser(invalidUser);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<RegistrationResponseDto>(badRequestResult.Value);

            Assert.False(response.IsSuccessfulRegistration);
            Assert.Contains("Entered invalid email", response.Errors!);
            _mockLogger.VerifyLog(LogLevel.Warning, 
                $"Could not registrate user with invalid email: {invalidUser}",
                Times.Once());
        }

        [Fact]
        public async Task RegisterUser_CreationFails_ReturnsBadRequestWithErrors()
        {
            // Arrange
            var validUser = _fixture.Build<UserForRegistrationDto>()
                .With(x => x.Email, "valid@example.com")
                .Create();

            var errors = new[] { "Password too weak", "Username taken" };
            _mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors.Select(e => 
                    new IdentityError { Description = e }).ToArray()));

            // Act
            var result = await _controller.RegisterUser(validUser);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<RegistrationResponseDto>(badRequestResult.Value);

            var errorsString = string.Join(", ", response.Errors!);
            Assert.Equal(errors, response.Errors);
            _mockLogger.VerifyLog(LogLevel.Warning, $"Error while creating user {validUser.Email}:" +
                $" {errorsString}", Times.Once());
        }

        [Fact]
        public async Task RegisterUser_ValidData_ReturnsCreated()
        {
            // Arrange
            var validUser = _fixture.Build<UserForRegistrationDto>()
                .With(x => x.Email, "valid@example.com")
                .Create();

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .ReturnsAsync("generated-token");

            // Act
            var result = await _controller.RegisterUser(validUser);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, ((StatusCodeResult)result).StatusCode);

            _mockLogger.VerifyLog(LogLevel.Information,
                $"Message with confirmation successfully sent to email {validUser.Email}",
                Times.Once());
            _mockLogger.VerifyLog(LogLevel.Information,
                $"For user {validUser.Email} granted role \"Visitor\"",
                Times.Once());
            _mockEmailSender.Verify(x => x.SendEmailAsync(It.IsAny<Message>()), Times.Once());
        }

        [Fact]
        public async Task EmailConfirmation_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var token = "test-token";

            _mockUserManager.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _controller.EmailConfirmation(email, token);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid email confirmation request", badRequestResult.Value);
            _mockLogger.VerifyLog(LogLevel.Warning, 
                $"User with email {email} not found",
                Times.Once());
        }

        [Fact]
        public async Task EmailConfirmation_DeletedUser_ReturnsBadRequest()
        {
            // Arrange
            var email = "deleted@example.com";
            var token = "test-token";
            var user = new User { Email = email, Deleted = true };

            _mockUserManager.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _controller.EmailConfirmation(email, token);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request", badRequestResult.Value);
            _mockLogger.VerifyLog(LogLevel.Warning, 
                $"An attempt was made to create a user {email} that is marked as \"Deleted\"",
                Times.Once());
        }

        [Fact]
        public async Task EmailConfirmation_InvalidToken_ReturnsBadRequest()
        {
            // Arrange
            var email = "valid@example.com";
            var token = "invalid-token";
            var user = new User { Email = email, Deleted = false };
            var errors = new[] { "Invalid token" };

            _mockUserManager.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(user);

            _mockUserManager.Setup(x => x.ConfirmEmailAsync(user, It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors.Select(e => 
                    new IdentityError { Description = e }).ToArray()));

            // Act
            var result = await _controller.EmailConfirmation(email, token);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid email confirmation request", badRequestResult.Value);
            _mockLogger.VerifyLog(LogLevel.Warning, $"Error while confirm user {email}",
                Times.Once());
        }

        [Fact]
        public async Task EmailConfirmation_ValidData_ReturnsOk()
        {
            // Arrange
            var email = "valid@example.com";
            var token = HttpUtility.UrlEncode("encoded-token");
            var user = new User { Email = email, Deleted = false };

            _mockUserManager.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(user);

            _mockUserManager.Setup(x => x.ConfirmEmailAsync(user, "encoded-token"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.EmailConfirmation(email, token);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockUserManager.Verify(x => x.ConfirmEmailAsync(user, "encoded-token"), Times.Once);
        }

        [Fact]
        public async Task Login_UserNotExists_ReturnsUnauthorized()
        {
            // Arrange
            var userDto = _fixture.Create<UserForAuthenticationDto>();
            _mockUserManager.Setup(x => x.FindByEmailAsync(userDto.Email!))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = Assert.IsType<AuthResponseDto>(unauthorizedResult.Value);
            Assert.Equal("Invalid authentication", response.ErrorMessage);

            _mockLogger.VerifyLog(LogLevel.Warning,
                $"User with email {userDto.Email} tried log in, but his doesn't exist",
                Times.Once());
        }

        [Fact]
        public async Task Login_UserDeleted_ReturnsBadRequest()
        {
            // Arrange
            var userDto = _fixture.Create<UserForAuthenticationDto>();
            var user = _fixture.Build<User>()
                .With(u => u.Deleted, true)
                .Create();

            _mockUserManager.Setup(x => x.FindByEmailAsync(userDto.Email!))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request", badRequestResult.Value);

            _mockLogger.VerifyLog(LogLevel.Warning,
                $"An attempt was made to log in a user {userDto.Email} that is marked as \"Deleted\"",
                Times.Once());
        }

        [Fact]
        public async Task Login_UserLockedOut_ReturnsUnauthorized()
        {
            // Arrange
            var userDto = _fixture.Create<UserForAuthenticationDto>();
            var user = _fixture.Build<User>()
                .With(x => x.Deleted, false)
                .Create();

            _mockUserManager.Setup(x => x.FindByEmailAsync(userDto.Email!))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.IsLockedOutAsync(user))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = Assert.IsType<AuthResponseDto>(unauthorizedResult.Value);
            Assert.Equal("The account is locked out", response.ErrorMessage);

            _mockLogger.VerifyLog(LogLevel.Warning,
                $"An attempt was made to log in a user {userDto.Email} that is locked out",
                Times.Once());
        }

        [Fact]
        public async Task Login_EmailNotConfirmed_ReturnsUnauthorized()
        {
            // Arrange
            var userDto = _fixture.Create<UserForAuthenticationDto>();
            var user = _fixture.Build<User>()
                .With(x => x.Deleted, false)
                .Create();

            _mockUserManager.Setup(x => x.FindByEmailAsync(userDto.Email!))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.IsLockedOutAsync(user))
                .ReturnsAsync(false);
            _mockUserManager.Setup(x => x.IsEmailConfirmedAsync(user))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = Assert.IsType<AuthResponseDto>(unauthorizedResult.Value);
            Assert.Equal("Email is not confirmed", response.ErrorMessage);

            _mockLogger.VerifyLog(LogLevel.Warning,
                $"An attempt was made to log in a user {userDto.Email} that is email not confirmed",
                Times.Once());
        }

        [Fact]
        public async Task Login_InvalidPasswordWithLockout_ReturnsUnauthorizedAndSendsEmail()
        {
            // Arrange
            var userDto = _fixture.Create<UserForAuthenticationDto>();
            var user = _fixture.Build<User>()
                .With(x => x.Deleted, false)
                .Create();

            _mockUserManager.Setup(x => x.FindByEmailAsync(userDto.Email!))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.IsLockedOutAsync(user))
                .ReturnsAsync(false);
            _mockUserManager.Setup(x => x.IsEmailConfirmedAsync(user))
                .ReturnsAsync(true);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, userDto.Password!))
                .ReturnsAsync(false);
            _mockUserManager.SetupSequence(x => x.IsLockedOutAsync(user))
                .ReturnsAsync(false) 
                .ReturnsAsync(true); 

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = Assert.IsType<AuthResponseDto>(unauthorizedResult.Value);
            Assert.Equal("The account is locked out", response.ErrorMessage);

            _mockLogger.VerifyLog(LogLevel.Warning,
                $"The user {userDto.Email} has reached the login attempt limit and has been locked out",
                Times.Once());
            _mockUserManager.Verify(x => x.AccessFailedAsync(user), Times.Once);
            _mockEmailSender.Verify(x => x.SendEmailAsync(It.IsAny<Message>()), Times.Once);
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            var userDto = _fixture.Create<UserForAuthenticationDto>();
            var user = _fixture.Build<User>()
                .With(x => x.Deleted, false)
                .Create();

            _mockUserManager.Setup(x => x.FindByEmailAsync(userDto.Email!))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.IsLockedOutAsync(user))
                .ReturnsAsync(false);
            _mockUserManager.Setup(x => x.IsEmailConfirmedAsync(user))
                .ReturnsAsync(true);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, userDto.Password!))
                .ReturnsAsync(false);
            _mockUserManager.Setup(x => x.IsLockedOutAsync(user))
                .ReturnsAsync(false); 

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = Assert.IsType<AuthResponseDto>(unauthorizedResult.Value);
            Assert.Equal("Invalid authentication", response.ErrorMessage);

            _mockLogger.VerifyLog(LogLevel.Warning,
                $"The user {userDto.Email} entered invalid password",
                Times.Once());
            _mockUserManager.Verify(x => x.AccessFailedAsync(user), Times.Once);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsAuthResponseWithTokens()
        {
            // Arrange
            var userDto = _fixture.Create<UserForAuthenticationDto>();
            var user = _fixture.Build<User>()
                .With(x => x.Deleted, false)
                .Create();
            var roles = _fixture.CreateMany<string>().ToArray();
            var accessToken = _fixture.Create<string>();
            var refreshToken = _fixture.Create<string>();

            _mockUserManager.Setup(x => x.FindByEmailAsync(userDto.Email!))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.IsLockedOutAsync(user))
                .ReturnsAsync(false);
            _mockUserManager.Setup(x => x.IsEmailConfirmedAsync(user))
                .ReturnsAsync(true);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, userDto.Password!))
                .ReturnsAsync(true);
            _mockUserManager.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(roles);
            _mockUserManager.Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            _mockJwtHandler.Setup(x => x.CreateAccessToken(user, roles))
                .Returns(accessToken);
            _mockJwtHandler.Setup(x => x.CreateRefreshToken())
                .Returns(refreshToken);

            // Act
            var result = await _controller.Login(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<AuthResponseDto>(okResult.Value);

            Assert.True(response.IsAuthSuccessful);
            Assert.Equal(accessToken, response.AccessToken);
            Assert.Equal(refreshToken, response.RefreshToken);

            Assert.Equal(refreshToken, user.RefreshToken);
            Assert.Equal(0, user.AccessFailedCount);

            _mockLogger.VerifyLog(LogLevel.Information,
                $"User with email {user.Email} is logged in",
                Times.Once());
            _mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task ForgotPassword_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var forgotPasswordDto = _fixture.Create<ForgotPasswordDto>();
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.ForgotPassword(forgotPasswordDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task ForgotPassword_UserNotExists_ReturnsBadRequestAndLogsWarning()
        {
            // Arrange
            var forgotPasswordDto = _fixture.Create<ForgotPasswordDto>();
            
            _mockUserManager.Setup(x => x.FindByEmailAsync(forgotPasswordDto.Email!))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _controller.ForgotPassword(forgotPasswordDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request", badRequestResult.Value);

            _mockLogger.VerifyLog(LogLevel.Warning,
                $"User with email {forgotPasswordDto.Email} tried to forgot password, " +
                $"but his doesn't exist in database",
                Times.Once());
        }

        [Fact]
        public async Task ForgotPassword_UserDeleted_ReturnsBadRequestAndLogsWarning()
        {
            // Arrange
            var forgotPasswordDto = _fixture.Create<ForgotPasswordDto>();
            var user = _fixture.Build<User>()
                .With(u => u.Deleted, true)
                .Create();

            _mockUserManager.Setup(x => x.FindByEmailAsync(forgotPasswordDto.Email!))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.ForgotPassword(forgotPasswordDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request", badRequestResult.Value);

            _mockLogger.VerifyLog(LogLevel.Warning,
                $"An attempt was made to forgot password a user {forgotPasswordDto.Email} " +
                $"that is marked as \"Deleted\"",
                Times.Once());
        }

        [Fact]
        public async Task ForgotPassword_ValidRequest_ReturnsOkAndLogsInformation()
        {
            // Arrange
            var forgotPasswordDto = _fixture.Create<ForgotPasswordDto>();
            var user = _fixture.Build<User>()
                .With(u => u.Deleted, false)
                .Create();
            var token = _fixture.Create<string>();

            _mockUserManager.Setup(x => x.FindByEmailAsync(forgotPasswordDto.Email!))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync(token);

            // Act
            var result = await _controller.ForgotPassword(forgotPasswordDto);

            // Assert
            Assert.IsType<OkResult>(result);

            _mockLogger.VerifyLog(LogLevel.Information,
                $"Successfully forgot password for user {forgotPasswordDto.Email}",
                Times.Once());

            var email = new MailboxAddress("Test mailbox", user.Email);

            _mockEmailSender.Verify(
                x => x.SendEmailAsync(It.Is<Message>(m =>
                    m.To.Contains(email) &&
                    m.Subject == "Reset password token" &&
                    m.Content.Contains(token))),
                Times.Once);
        }

        [Fact]
        public async Task ForgotPassword_ValidRequest_CreatesCorrectCallbackUrl()
        {
            // Arrange
            var forgotPasswordDto = _fixture.Build<ForgotPasswordDto>()
                .With(x => x.ClientUri, "https://example.com/reset-password")
                .Create();
            var user = _fixture.Build<User>()
                .With(u => u.Deleted, false)
                .Create();
            var token = _fixture.Create<string>();

            _mockUserManager.Setup(x => x.FindByEmailAsync(forgotPasswordDto.Email!))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync(token);

            string? actualCallback = null;
            _mockEmailSender.Setup(x => x.SendEmailAsync(It.IsAny<Message>()))
                .Callback<Message>(m => actualCallback = m.Content);

            // Act
            await _controller.ForgotPassword(forgotPasswordDto);

            // Assert
            Assert.NotNull(actualCallback);
            Assert.Contains(forgotPasswordDto.ClientUri!, actualCallback);
            Assert.Contains($"token={token}", actualCallback);
            Assert.Contains($"email={forgotPasswordDto.Email}", actualCallback);
        }
    }
}
