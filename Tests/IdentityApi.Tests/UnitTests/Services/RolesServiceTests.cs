using AutoFixture;
using AutoFixture.AutoMoq;
using MicroZoo.IdentityApi.Repositories;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Roles;
using Moq;

namespace MicroZoo.IdentityApi.Tests.UnitTests.Services
{
    public class RolesServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRolesRepository> _rolesRepository;
        private readonly Mock<IUserRolesService> _userRolesService;
        private readonly Mock<IRoleRequirementsService> _roleRequirementsService;
        private readonly RolesService _rolesService;

        public RolesServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _rolesRepository = _fixture.Freeze<Mock<IRolesRepository>>();
            _userRolesService = _fixture.Freeze<Mock<IUserRolesService>>();
            _roleRequirementsService = _fixture.Freeze<Mock<IRoleRequirementsService>>();

            _rolesService = _fixture.Create<RolesService>();
        }

        [Fact]
        public async Task GetAllRolesAsync_ReturnsGetRolesResponse()
        {
            // Arrange
            var roles = _fixture.Build<Role>()
                .OmitAutoProperties()
                .CreateMany(5)
                .ToList();

            _rolesRepository.Setup(x => x.GetAllRolesAsync()).ReturnsAsync(roles);

            // Act
            var result = await _rolesService.GetAllRolesAsync();

            // Assert
            Assert.NotNull(result.Roles);
            Assert.Equal(5, result.Roles.Count);
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abcd")]
        [InlineData("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxxx")]
        public async Task GetRoleAsync_NotGuid_ReturnsRoleIdIsNotGuid(string roleId)
        {
            // Arrange            
            var expectedMessage = "Role Id is not Guid";

            // Act
            var result = await _rolesService.GetRoleAsync(roleId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task GetRoleAsync_RoleIsNotExist_ReturnsRoleDoesNotExist()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var expectedMessage = $"Role with Id {roleId} does not exist";

            _rolesRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync((Role)null!);

            // Act
            var result = await _rolesService.GetRoleAsync(roleId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task GetRoleAsync_CorrectRequest_ReturnsGetRoleResponse()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var selectedRole = _fixture.Build<Role>()
                .With(x => x.Id, roleId)
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .Create();
            var expectedMessage = $"Role with Id {roleId} does not exist";

            _rolesRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(selectedRole);

            // Act
            var result = await _rolesService.GetRoleAsync(roleId);

            // Assert
            Assert.NotNull(result.Role);
        }

        [Fact]
        public async Task AddRoleAsync_NullRoleWithoutIdDto_ReturnsNewRoleMustBeNotNull()
        {
            // Arrange            
            var expectedMessage = "New role must be not null";

            // Act
            var result = await _rolesService.AddRoleAsync(null!);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task AddRoleAsync_NullRole_ReturnsNotExpectedError()
        {
            // Arrange            
            var roleWithoutIdDto = _fixture.Build<RoleWithoutIdDto>().Create();
            var expectedMessage = "Not expected error during creation role";

            _rolesRepository.Setup(x => x.AddRoleAsync(It.IsAny<Role>()))
                .ReturnsAsync((Role)null!);

            // Act
            var result = await _rolesService.AddRoleAsync(roleWithoutIdDto);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task AddRoleAsync_CorrectRequest_ReturnsGetRoleResponse()
        {
            // Arrange            
            var roleWithoutIdDto = _fixture.Build<RoleWithoutIdDto>().Create();
            var createdRole = _fixture.Build<Role>()
                .OmitAutoProperties()
                .Create();

            _rolesRepository.Setup(x => x.AddRoleAsync(It.IsAny<Role>()))
                .ReturnsAsync(createdRole);

            // Act
            var result = await _rolesService.AddRoleAsync(roleWithoutIdDto);

            // Assert
            Assert.NotNull(result.Role);
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abcd")]
        [InlineData("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxxx")]
        public async Task UpdateRoleAsync_NotGuid_ReturnsRoleIdIsNotGuid(string roleId)
        {
            // Arrange            
            var roleWithoutIdDto = _fixture.Build<RoleWithoutIdDto>().Create();
            var expectedMessage = "Role Id is not Guid";

            // Act
            var result = await _rolesService.UpdateRoleAsync(roleId, roleWithoutIdDto);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateRoleAsync_RoleNotExist_ReturnsRoleDoesNotExist()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var roleWithoutIdDto = _fixture.Build<RoleWithoutIdDto>().Create();            
            var expectedMessage = $"Role with Id {roleId} does not exist";

            _rolesRepository.Setup(x => x.UpdateRoleAsync(It.IsAny<string>(), It.IsAny<Role>()))
                .ReturnsAsync((Role)null!);

            // Act
            var result = await _rolesService.UpdateRoleAsync(roleId, roleWithoutIdDto);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateRoleAsync_CorrectRequest_ReturnsGetRoleResponse()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var roleWithoutIdDto = _fixture.Build<RoleWithoutIdDto>().Create();
            var updatedRole = _fixture.Build<Role>()
                .With(x => x.Id, roleId)
                .With(x => x.NormalizedName, roleWithoutIdDto.NormalizedName)
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .Create();

            _rolesRepository.Setup(x => x.UpdateRoleAsync(It.IsAny<string>(), It.IsAny<Role>()))
                .ReturnsAsync(updatedRole);

            // Act
            var result = await _rolesService.UpdateRoleAsync(roleId, roleWithoutIdDto);

            // Assert
            Assert.NotNull(result.Role);
            Assert.Equal(roleWithoutIdDto.NormalizedName, result.Role.NormalizedName);
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abcd")]
        [InlineData("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxxx")]
        public async Task SoftDeleteRoleAsync_NotGuid_ReturnsRoleIdIsNotGuid(string roleId)
        {
            // Arrange            
            var expectedMessage = "Role Id is not Guid";

            // Act
            var result = await _rolesService.SoftDeleteRoleAsync(roleId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task SoftDeleteRoleAsync_RoleNotExist_ReturnsRoleDoesNotExist()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var expectedMessage = $"Role with Id {roleId} does not exist";

            _rolesRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync((Role)null!);

            // Act
            var result = await _rolesService.SoftDeleteRoleAsync(roleId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task SoftDeleteRoleAsync_UnsuccessfullyDeletedUserRoles_ReturnsInnerServerError()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var deletedRole = _fixture.Build<Role>()
                .With(x => x.Id, roleId)
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .Create();
            var expectedMessage = $"Inner server error";

            _rolesRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(deletedRole);
            _userRolesService.Setup(x => x.DeleteUserRolesByRoleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(false);
            _roleRequirementsService.Setup(x =>
            x.DeleteRoleRequirementsByRoleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _rolesService.SoftDeleteRoleAsync(roleId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task SoftDeleteRoleAsync_UnsuccessfullyDeletedRoleRequirements_ReturnsInnerServerError()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var deletedRole = _fixture.Build<Role>()
                .With(x => x.Id, roleId)
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .Create();
            var expectedMessage = $"Inner server error";

            _rolesRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(deletedRole);
            _userRolesService.Setup(x => x.DeleteUserRolesByRoleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _roleRequirementsService.Setup(x =>
            x.DeleteRoleRequirementsByRoleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _rolesService.SoftDeleteRoleAsync(roleId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task SoftDeleteRoleAsync_CorrectRequest_ReturnsGetRoleResponse()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var deletedRole = _fixture.Build<Role>()
                .With(x => x.Id, roleId)
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .Create();

            _rolesRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(deletedRole);
            _userRolesService.Setup(x => x.DeleteUserRolesByRoleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _roleRequirementsService.Setup(x =>
                x.DeleteRoleRequirementsByRoleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _rolesRepository.Setup(x => x.SoftDeleteRoleAsync(It.IsAny<Role>()))
                .ReturnsAsync(deletedRole);

            // Act
            var result = await _rolesService.SoftDeleteRoleAsync(roleId);

            // Assert
            Assert.NotNull(result.Role);
        }
    }
}
