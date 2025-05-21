using AutoFixture;
using AutoFixture.AutoMoq;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MicroZoo.IdentityApi.Consumers;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.Infrastructure.MassTransit.Requests.IdentityApi;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Users;
using Moq;
using System.Security.Claims;

namespace MicroZoo.IdentityApi.Tests.UnitTests
{
    public class CheckAccessConsumerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ConsumeContext<CheckAccessRequest>> _mockContext;
        private readonly Mock<IJwtHandler> _mockJwtHandler;        
        private readonly Mock<ILogger<CheckAccessConsumer>> _mockLogger;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly CheckAccessConsumer _consumer;        

        public CheckAccessConsumerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockContext = _fixture.Freeze<Mock<ConsumeContext<CheckAccessRequest>>>();
            _mockJwtHandler = _fixture.Freeze<Mock<IJwtHandler>>();            
            _mockLogger = _fixture.Freeze<Mock<ILogger<CheckAccessConsumer>>>();

            _mockUserManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                null, // options
                null, // password hasher
                null, // user validators
                null, // password validators
                null, // key normalizer
                null, // errors
                null, // services
                null  // logger
            );

            _fixture.Inject(_mockUserManager.Object);

            _consumer = _fixture.Create<CheckAccessConsumer>();
        }
        

        [Fact]
        public async Task Consume_InvalidToken_ReturnsNotAuthenticated()
        {
            // Arrange
            var request = _fixture.Build<CheckAccessRequest>()
                .With(x => x.AccessToken, "invalid_token")
                .Create();

            _mockContext.Setup(x => x.Message).Returns(request);
            _mockJwtHandler.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns((ClaimsPrincipal)null);

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
            var AccessToken = "invalid_token";
            _mockContext.Verify(x => x.RespondAsync(It.Is<CheckAccessResponse>(r =>
                !r.IsAuthenticated)), Times.Once);

            _mockLogger.VerifyLog(LogLevel.Information,
                "User with access token invalid_token not found", Times.Once());
        }

        [Fact]
        public async Task Consume_UnauthenticatedUser_ReturnsNotAuthenticated()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test_user")};
            var identity = new ClaimsIdentity(claims);            
            var principal = new ClaimsPrincipal(identity);            
            var request = _fixture.Create<CheckAccessRequest>();

            _mockContext.Setup(x => x.Message).Returns(request);
            _mockJwtHandler.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(principal);

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
            _mockContext.Verify(x => x.RespondAsync(It.Is<CheckAccessResponse>(r =>
                !r.IsAuthenticated)), Times.Once);
            _mockLogger.VerifyLog(LogLevel.Information,
                "User Test_user is not authenticated", Times.Once());
        }

        [Fact]
        public async Task Consume_NullPolicies_ReturnsAccessNotConfirmed()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test_user") };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            var request = _fixture.Build<CheckAccessRequest>()
                .With(x => x.Policies, (List<string>)null!)
                .Create();

            _mockContext.Setup(x => x.Message).Returns(request);
            _mockJwtHandler.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(principal);

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
            _mockContext.Verify(x => x.RespondAsync(It.Is<CheckAccessResponse>(r =>
                !r.IsAccessConfirmed)), Times.Once);
            _mockLogger.VerifyLog(LogLevel.Information,
                "Access for user Test_user is not confirmed", Times.Once());
        }

        [Fact]
        public async Task Consume_NoPolicies_ReturnsAccessNotConfirmed()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test_user") };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            var request = _fixture.Build<CheckAccessRequest>()
                .With(x => x.Policies, new List<string>())
                .Create();

            _mockContext.Setup(x => x.Message).Returns(request);
            _mockJwtHandler.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(principal);

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
            _mockContext.Verify(x => x.RespondAsync(It.Is<CheckAccessResponse>(r =>
                !r.IsAccessConfirmed)), Times.Once);
            _mockLogger.VerifyLog(LogLevel.Information,
                "Access for user Test_user is not confirmed", Times.Once());
        }

        [Fact]
        public async Task Consume_UserAbsentInDb_ReturnsUserNotFound()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test_user") };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            var request = _fixture.Build<CheckAccessRequest>()
                .With(x => x.Policies, new List<string> { "test_requirement" })
                .Create();

            _mockContext.Setup(x => x.Message).Returns(request);
            _mockJwtHandler.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(principal);            
            _mockUserManager.Setup(x => x.FindByNameAsync("Test_user"))
                .ReturnsAsync((User)null!);

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
            _mockContext.Verify(x => x.RespondAsync(It.IsAny<CheckAccessResponse>()), Times.Once);
            _mockLogger.VerifyLog(LogLevel.Information,
                "User Test_user not found", Times.Once());
        }
    }
}
