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
using System.Net;
using System.Security.Claims;
using System.Security.Principal;

namespace MicroZoo.IdentityApi.Tests.UnitTests.Controllers
{
    public class RolesControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRolesService> _mockRolesService;
        private readonly Mock<IRoleRequirementsService> _mockRoleRequirementsService;
        private readonly Mock<ILogger<RolesController>> _mockLogger;
        private readonly Mock<IJwtHandler> _mockJwtHandler;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly RolesController _controller;

        public RolesControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Customize<RolesController>(c => c
                .OmitAutoProperties()
                .Without(x => x.ControllerContext));

            _mockRolesService = _fixture.Freeze<Mock<IRolesService>>();
            _mockRoleRequirementsService = _fixture.Freeze<Mock<IRoleRequirementsService>>();
            _mockLogger = _fixture.Freeze<Mock<ILogger<RolesController>>>();
            _mockJwtHandler = _fixture.Freeze<Mock<IJwtHandler>>();
            _mockHttpContext = _fixture.Freeze<Mock<HttpContext>>();

            _controller = _fixture.Create<RolesController>();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };
        }

        [Fact]
        public void GetAllRolesAsync_HasAuthorizeAttributeWithCorrectPolicy()
        {
            // Act & Assert
            Assert.True(AuthorizeAttributeHandler
                .HasAuthorizeAttributeWithPolicy<RolesController>(
                "GetAllRolesAsync", "IdentityApi.Read"));
        }

        [Fact]
        public async Task GetAllRolesAsync_WhenServiceReturnsError_ReturnsBadRequest()
        {
            // Arrange
            var errorMessage = "Failed to retrieve roles";
            var response = new GetRolesResponse { ErrorMessage = errorMessage };

            _mockRolesService.Setup(x => x.GetAllRolesAsync())
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllRolesAsync();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task GetAllRolesAsync_CorrectRequest_ReturnsCorrectResult()
        {
            // Arrange
            var roles = _fixture.Build<Role>()
                .OmitAutoProperties()
                .CreateMany(5)
                .ToList();
            var response = new GetRolesResponse { Roles = roles };

            _mockRolesService.Setup(x => x.GetAllRolesAsync())
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllRolesAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRoles = Assert
                .IsAssignableFrom<IEnumerable<Role>>(okResult.Value);
            Assert.Equal(5, returnedRoles.Count());
        }

        [Fact]
        public void GetRoleAsync_HasAuthorizeAttributeWithCorrectPolicy()
        {
            // Act & Assert
            Assert.True(AuthorizeAttributeHandler
                .HasAuthorizeAttributeWithPolicy<RolesController>(
                "GetRoleAsync", "IdentityApi.Read"));
        }

        [Fact]
        public async Task GetRoleAsync_WhenServiceReturnsError_ReturnsBadRequest()
        {
            // Arrange
            var roleId = Guid.NewGuid().ToString();
            var errorMessage = "Failed to retrieve role";
            var response = new GetRoleResponse { ErrorMessage = errorMessage };

            _mockRolesService.Setup(x => x.GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetRoleAsync(roleId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task GetRoleAsync_CorrectRequest_ReturnsCorrectResult()
        {
            // Arrange
            var roleId = Guid.NewGuid().ToString();
            var role = _fixture.Build<Role>()
                .OmitAutoProperties()                
                .Create();
            var response = new GetRoleResponse { Role = role };

            _mockRolesService.Setup(x => x.GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetRoleAsync(roleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRoles = Assert
                .IsAssignableFrom<Role>(okResult.Value);
        }

        [Fact]
        public void AddRoleAsync_HasAuthorizeAttributeWithCorrectPolicy()
        {
            // Act & Assert
            Assert.True(AuthorizeAttributeHandler
                .HasAuthorizeAttributeWithPolicy<RolesController>(
                "AddRoleAsync", "IdentityApi.Create"));
        }

        [Fact]
        public async Task AddRoleAsync_NullDto_ReturnsBadRequestAndLogsWarning()
        {
            // Arrange
            var ipAddress = "192.168.1.1";
            var port = 1234;
            _mockHttpContext.Setup(x => x.Connection.RemoteIpAddress)
                .Returns(IPAddress.Parse(ipAddress));
            _mockHttpContext.Setup(x => x.Connection.RemotePort)
                .Returns(port);

            // Act
            var result = await _controller.AddRoleAsync(null!);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid request", badRequestResult.Value);

            _mockLogger.VerifyLog(LogLevel.Warning,
                $"Invalid RoleWithoutIdDto sent from address {ipAddress}:{port}",
                Times.Once());
        }

        [Fact]
        public async Task AddRoleAsync_ServiceReturnsError_ReturnsBadRequestAndLogsError()
        {
            // Arrange
            var dto = _fixture.Create<RoleWithoutIdDto>();
            var errorMessage = "Role already exists";
            var response = new GetRoleResponse { ErrorMessage = errorMessage };

            _mockRolesService.Setup(x => x.AddRoleAsync(It.IsAny<RoleWithoutIdDto>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.AddRoleAsync(dto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);

            _mockLogger.VerifyLog(LogLevel.Information,
                $"An error occurred while adding role: {errorMessage}",
                Times.Once());
        }

        [Fact]
        public async Task AddRoleAsync_ValidDto_ReturnsOkWithRoleAndLogsSuccess()
        {
            // Arrange
            var dto = _fixture.Create<RoleWithoutIdDto>();
            var role = _fixture.Build<Role>()
                .OmitAutoProperties()
                .Create();
            var response = new GetRoleResponse { Role = role };
            var adminName = _fixture.Create<string>();
            var principal = new ClaimsPrincipal(new GenericIdentity(adminName));

            _mockRolesService.Setup(x => x.AddRoleAsync(It.IsAny<RoleWithoutIdDto>()))
                .ReturnsAsync(response);
            _mockJwtHandler.Setup(x => x.GetPrincipalFromHttpRequest(It.IsAny<HttpRequest>()))
                .Returns(principal);

            // Act
            var result = await _controller.AddRoleAsync(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(role, okResult.Value);

            _mockLogger.VerifyLog(LogLevel.Information,
                $"The user {principal.Identity!.Name} created new role {dto}",
                Times.Once());
        }

        [Fact]
        public void UpdateRoleAsync_HasAuthorizeAttributeWithCorrectPolicy()
        {
            // Act & Assert
            Assert.True(AuthorizeAttributeHandler
                .HasAuthorizeAttributeWithPolicy<RolesController>(
                "UpdateRoleAsync", "IdentityApi.Update"));
        }
    }
}
