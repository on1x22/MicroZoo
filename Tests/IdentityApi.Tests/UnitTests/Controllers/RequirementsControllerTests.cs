using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MicroZoo.IdentityApi.Controllers;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Roles;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace MicroZoo.IdentityApi.Tests.UnitTests.Controllers
{
    public class RequirementsControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRequirementsService> _mockRequirementsService;
        private readonly Mock<ILogger<RequirementsController>> _mockLogger;
        private readonly Mock<IJwtHandler> _mockJwtHandler;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly RequirementsController _controller;

        public RequirementsControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _fixture.Customize<RequirementsController>(c => c
                .OmitAutoProperties()
                .Without(x => x.ControllerContext));

            _mockRequirementsService = _fixture.Freeze<Mock<IRequirementsService>>();
            _mockLogger = _fixture.Freeze<Mock<ILogger<RequirementsController>>>();
            _mockJwtHandler = _fixture.Freeze<Mock<IJwtHandler>>();
            _mockHttpContext = new Mock<HttpContext>();

            _controller = _fixture.Create<RequirementsController>();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };
        }

        [Fact]
        public void GetAllRequirementsAsync_HasAuthorizeAttributeWithCorrectPolicy()
        {
            // Act & Assert
            Assert.True(AuthorizeAttributeHandler
                .HasAuthorizeAttributeWithPolicy<RequirementsController>(
                "GetAllRequirementsAsync", "IdentityApi.Read"));
        }

        [Fact]
        public async Task GetAllRequirementsAsync_WhenServiceReturnsError_ReturnsBadRequest()
        {
            // Arrange
            var errorMessage = "Failed to retrieve requirements";
            var response = new GetRequirementsResponse { ErrorMessage = errorMessage };

            _mockRequirementsService.Setup(x => x.GetAllRequirementsAsync())
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllRequirementsAsync();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task GetAllRequirementsAsync_WithDifferentCounts_ReturnsCorrectResult()
        {
            // Arrange
            var requirements = _fixture.Build<Requirement>()
                .OmitAutoProperties()
                .CreateMany(5)
                .ToList();
            var response = new GetRequirementsResponse { Requirements = requirements };

            _mockRequirementsService.Setup(x => x.GetAllRequirementsAsync())
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllRequirementsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRequirements = Assert
                .IsAssignableFrom<IEnumerable<Requirement>>(okResult.Value);
            Assert.Equal(5, returnedRequirements.Count());
        }

        [Fact]
        public void GetRequirementAsync_HasAuthorizeAttributeWithCorrectPolicy()
        {
            // Act & Assert
            Assert.True(AuthorizeAttributeHandler
                .HasAuthorizeAttributeWithPolicy<RequirementsController>(
                "GetRequirementAsync", "IdentityApi.Read"));
        }

        [Fact]
        public async Task GetRequirementAsync_WhenServiceReturnsError_ReturnsBadRequest()
        {
            // Arrange
            var requirementId = Guid.NewGuid();
            var errorMessage = "Failed to retrieve requirement";
            var response = new GetRequirementResponse { ErrorMessage = errorMessage };

            _mockRequirementsService.Setup(x => x.GetRequirementAsync(It.IsAny<Guid>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetRequirementAsync(requirementId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task GetRequirementAsync_CorrectData_ReturnsCorrectResult()
        {
            // Arrange
            var requirementId = Guid.NewGuid();
            var requirement = _fixture.Build<Requirement>()
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .OmitAutoProperties()
                .Create();
            var response = new GetRequirementResponse { Requirement = requirement };

            _mockRequirementsService.Setup(x => x.GetRequirementAsync(It.IsAny<Guid>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetRequirementAsync(requirementId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRequirement = Assert
                .IsAssignableFrom<Requirement>(okResult.Value);
        }

        [Fact]
        public void AddRequirementAsync_HasAuthorizeAttributeWithCorrectPolicy()
        {
            // Act & Assert
            Assert.True(AuthorizeAttributeHandler
                .HasAuthorizeAttributeWithPolicy<RequirementsController>(
                "AddRequirementAsync", "IdentityApi.Create"));
        }

        [Fact]
        public async Task AddRequirementAsync_NullDto_ReturnsBadRequestAndLogsWarning()
        {
            // Arrange
            var ipAddress = "123.123.123.123";
            _mockHttpContext.Setup(x => x.Connection.RemoteIpAddress)
                .Returns(System.Net.IPAddress.Parse(ipAddress));

            // Act
            var result = await _controller.AddRequirementAsync(null!);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request", badRequestResult.Value);

            _mockLogger.VerifyLog(LogLevel.Warning,
                $"Invalid RequirementWithoutIdDto sent from address {ipAddress}",
                Times.Once());
        }

        [Fact]
        public async Task AddRequirementAsync_ServiceReturnsError_ReturnsBadRequest()
        {
            // Arrange
            var dto = _fixture.Create<RequirementWithoutIdDto>();
            var errorMessage = "Validation failed";
            var response = new GetRequirementResponse { ErrorMessage = errorMessage };

            _mockRequirementsService.Setup(x => x
                .AddRequirementAsync(It.IsAny<RequirementWithoutIdDto>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.AddRequirementAsync(dto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);

            _mockLogger.VerifyLog(LogLevel.Information,
                $"An error occurred while adding requirement: {errorMessage}",
                Times.Once());
        }

        [Fact]
        public async Task AddRequirementAsync_ValidDto_ReturnsOkWithRequirement()
        {
            // Arrange
            var requirementDto = _fixture.Create<RequirementWithoutIdDto>();
            var requirement = _fixture.Build<Requirement>()
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)                
                .Create();
            var response = new GetRequirementResponse { Requirement = requirement };            
            var adminName = _fixture.Create<string>();
            var principal = new ClaimsPrincipal(new GenericIdentity(adminName));

            _mockRequirementsService.Setup(x => x
                .AddRequirementAsync(It.IsAny<RequirementWithoutIdDto>()))
                .ReturnsAsync(response);
            _mockJwtHandler.Setup(x => x.GetPrincipalFromHttpRequest(It.IsAny<HttpRequest>()))
                .Returns(principal);

            // Act
            var result = await _controller.AddRequirementAsync(requirementDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(requirement, okResult.Value);

            _mockLogger.VerifyLog(LogLevel.Information,
                $"The user {principal.Identity!.Name} created new requirement {requirementDto}",
                Times.Once());
        }

        [Fact]
        public void SoftDeleteRequirementAsync_HasAuthorizeAttributeWithCorrectPolicy()
        {
            // Act & Assert
            Assert.True(AuthorizeAttributeHandler
                .HasAuthorizeAttributeWithPolicy<RequirementsController>(
                "SoftDeleteRequirementAsync", "IdentityApi.Delete"));
        }

        [Fact]
        public async Task SoftDeleteRequirementAsync_ServiceReturnsError_ReturnsBadRequest()
        {
            // Arrange
            var requirementId = Guid.NewGuid();
            var errorMessage = "Requirement not found";
            var response = new GetRequirementResponse { ErrorMessage = errorMessage };
            var adminName = _fixture.Create<string>();
            var principal = new ClaimsPrincipal(new GenericIdentity(adminName));

            _mockJwtHandler.Setup(x => x.GetPrincipalFromHttpRequest(It.IsAny<HttpRequest>()))
                .Returns(principal);
            _mockRequirementsService.Setup(x => x.SoftDeleteRequirementAsync(It.IsAny<Guid>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.SoftDeleteRequirementAsync(requirementId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);

            _mockLogger.VerifyLog(LogLevel.Information,
                $"An error occurred while deleting role: {errorMessage}",
                Times.Once());
        }

        [Fact]
        public async Task SoftDeleteRequirementAsync_ValidId_ReturnsOkAndLogsSuccess()
        {
            // Arrange
            var requirementId = Guid.NewGuid();
            var requirement = _fixture.Build<Requirement>()
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .Create();
            var response = new GetRequirementResponse { Requirement = requirement };
            var adminName = _fixture.Create<string>();
            var principal = new ClaimsPrincipal(new GenericIdentity(adminName));

            _mockJwtHandler.Setup(x => x.GetPrincipalFromHttpRequest(It.IsAny<HttpRequest>()))
                .Returns(principal);
            _mockRequirementsService.Setup(x => x.SoftDeleteRequirementAsync(It.IsAny<Guid>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.SoftDeleteRequirementAsync(requirementId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(requirement, okResult.Value);

            _mockLogger.VerifyLog(LogLevel.Information,
                $"The user {principal.Identity!.Name} deleted requirement with Id {requirementId}",
                Times.Once());
        }
    }
}
