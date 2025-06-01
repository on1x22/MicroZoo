using AutoFixture;
using AutoFixture.AutoMoq;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MicroZoo.IdentityApi.Consumers;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.IdentityApi;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Users;
using Moq;
using System.Security.Claims;

namespace MicroZoo.IdentityApi.Tests.UnitTests.Consumers
{
    public class CheckAccessConsumerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ConsumeContext<CheckAccessRequest>> _mockContext;
        private readonly Mock<IJwtHandler> _mockJwtHandler;        
        private readonly Mock<ILogger<CheckAccessConsumer>> _mockLogger;
        private readonly Mock<IUserRequirementsService> _mockUserRequirementsService;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly CheckAccessConsumer _consumer;        

        public CheckAccessConsumerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockContext = _fixture.Freeze<Mock<ConsumeContext<CheckAccessRequest>>>();
            _mockJwtHandler = _fixture.Freeze<Mock<IJwtHandler>>();            
            _mockLogger = _fixture.Freeze<Mock<ILogger<CheckAccessConsumer>>>();
            _mockUserRequirementsService = _fixture.Freeze<Mock<IUserRequirementsService>>();

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
                .Returns((ClaimsPrincipal)null!);

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
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
        public async Task Consume_UserAbsentInDb_ReturnsNotAuthenticated()
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
            _mockContext.Verify(x => x.RespondAsync(It.Is<CheckAccessResponse>(r =>
                !r.IsAuthenticated)), Times.Once);
            _mockLogger.VerifyLog(LogLevel.Information,
                "User Test_user not found", Times.Once());
        }

        [Fact]
        public async Task Consume_DeletedUser_ReturnsNotAuthenticated()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test_user") };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            var user = _fixture.Build<User>()
                .With(x => x.UserName, "Test_user")
                .With(x => x.Deleted, true)
                .Create();
            var request = _fixture.Build<CheckAccessRequest>()
                .With(x => x.Policies, new List<string> { "test_requirement" })
                .Create();

            _mockContext.Setup(x => x.Message).Returns(request);
            _mockJwtHandler.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(principal);
            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
            _mockContext.Verify(x => x.RespondAsync(It.Is<CheckAccessResponse>(r =>
                !r.IsAuthenticated)), Times.Once);
            _mockLogger.VerifyLog(LogLevel.Information,
                "Status of the user Test_user is \"Deleted\"", Times.Once());
        }

        [Fact]
        public async Task Consume_PolicyIsNull_ReturnsAccessNotConfirmed()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test_user") };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            var user = _fixture.Build<User>()
                .With(x => x.UserName, "Test_user")
                .With(x => x.Deleted, false)
                .Create();
            var request = _fixture.Build<CheckAccessRequest>()
                .With(x => x.Policies, new List<string> { "test_requirement" })
                .Create();

            _mockContext.Setup(x => x.Message).Returns(request);
            _mockJwtHandler.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(principal);
            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUserRequirementsService.Setup(x => x.GetAllowedRequirementsOfUser(It.IsAny<User>()))
                .ReturnsAsync((List<string>)null!);

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
            _mockContext.Verify(x => x.RespondAsync(It.Is<CheckAccessResponse>(r =>
                !r.IsAccessConfirmed)), Times.Once);
            _mockLogger.VerifyLog(LogLevel.Information,
                "User Test_user have null requirements", Times.Once());
        }

        [Fact]
        public async Task Consume_ZeroListOfPolicies_ReturnsAccessNotConfirmed()
        {            
            // Arrange
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test_user") };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            var user = _fixture.Build<User>()
                .With(x => x.UserName, "Test_user")
                .With(x => x.Deleted, false)
                .Create();
            var request = _fixture.Build<CheckAccessRequest>()
                .With(x => x.Policies, new List<string> { "test_requirement" })
                .Create();

            _mockContext.Setup(x => x.Message).Returns(request);
            _mockJwtHandler.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(principal);
            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUserRequirementsService.Setup(x => x.GetAllowedRequirementsOfUser(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
            _mockContext.Verify(x => x.RespondAsync(It.Is<CheckAccessResponse>(r =>
                !r.IsAccessConfirmed)), Times.Once);
            _mockLogger.VerifyLog(LogLevel.Information,
                "User Test_user have not necessary requirements", Times.Once());
        }

        [Fact]
        public async Task Consume_AllPoliciesMismatch_ReturnsAccessNotConfirmed()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test_user") };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            var user = _fixture.Build<User>()
                .With(x => x.UserName, "Test_user")
                .With(x => x.Deleted, false)
                .Create();
            var request = _fixture.Build<CheckAccessRequest>()
                .With(x => x.Policies, new List<string> { "test_requirement_1", "test_requirement_2" })
                .Create();

            _mockContext.Setup(x => x.Message).Returns(request);
            _mockJwtHandler.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(principal);
            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUserRequirementsService.Setup(x => x.GetAllowedRequirementsOfUser(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { "test_requirement_3", "test_requirement_4" });

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
            _mockContext.Verify(x => x.RespondAsync(It.Is<CheckAccessResponse>(r =>
                !r.IsAccessConfirmed)), Times.Once);
            _mockLogger.VerifyLog(LogLevel.Information,
                "User Test_user have not necessary requirements", Times.Once());
        }

        [Fact]
        public async Task Consume_OnePolicyMatch_ReturnsAccessConfirmed()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test_user") };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            var user = _fixture.Build<User>()
                .With(x => x.UserName, "Test_user")
                .With(x => x.Deleted, false)
                .Create();
            var request = _fixture.Build<CheckAccessRequest>()
                .With(x => x.Policies, new List<string> { "test_requirement_1", "test_requirement_2" })
                .Create();

            _mockContext.Setup(x => x.Message).Returns(request);
            _mockJwtHandler.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(principal);
            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUserRequirementsService.Setup(x => x.GetAllowedRequirementsOfUser(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { "test_requirement_1", "test_requirement_4" });

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
            _mockContext.Verify(x => x.RespondAsync(It.Is<CheckAccessResponse>(r =>
                r.IsAuthenticated && r.IsAccessConfirmed)), Times.Once);
            _mockLogger.VerifyLog(LogLevel.Information,
                "Access confirm for user Test_user", Times.Once());
        }

        [Fact]
        public async Task Consume_ValidRequest_ReturnsAccessConfirmed()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test_user") };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            var user = _fixture.Build<User>()
                .With(x => x.UserName, "Test_user")
                .With(x => x.Deleted, false)
                .Create();
            var request = _fixture.Build<CheckAccessRequest>()
                .With(x => x.Policies, new List<string> { "test_requirement" })
                .Create();

            _mockContext.Setup(x => x.Message).Returns(request);
            _mockJwtHandler.Setup(x => x.GetPrincipalFromToken(It.IsAny<string>()))
                .Returns(principal);
            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUserRequirementsService.Setup(x => x.GetAllowedRequirementsOfUser(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { "test_requirement", "requirement_test" });

            // Act
            await _consumer.Consume(_mockContext.Object);

            // Assert
            _mockContext.Verify(x => x.RespondAsync(It.Is<CheckAccessResponse>(r =>
                r.IsAuthenticated && r.IsAccessConfirmed)), Times.Once);
            _mockLogger.VerifyLog(LogLevel.Information,
                "Access confirm for user Test_user", Times.Once());
        }
    }
}
