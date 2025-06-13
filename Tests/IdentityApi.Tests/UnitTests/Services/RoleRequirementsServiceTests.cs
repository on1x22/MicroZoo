using AutoFixture;
using AutoFixture.AutoMoq;
using MicroZoo.IdentityApi.Repositories;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Roles;
using Moq;

namespace MicroZoo.IdentityApi.Tests.UnitTests.Services
{
    public class RoleRequirementsServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRoleRequirementsRepository> _roleRequirementsRepository;
        private readonly Mock<IRolesRepository> _rolesRepository;
        private readonly Mock<IRequirementsRepository> _requirementsRepository;
        private readonly RoleRequirementsService _roleRequirementsService;

        public RoleRequirementsServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _roleRequirementsRepository = _fixture.Freeze<Mock<IRoleRequirementsRepository>>();
            _rolesRepository = _fixture.Freeze<Mock<IRolesRepository>>();
            _requirementsRepository = _fixture.Freeze<Mock<IRequirementsRepository>>();

            _roleRequirementsService = _fixture.Create<RoleRequirementsService>();
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abcd")]
        [InlineData("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxxx")]
        public async Task GetRoleWithRequirementsAsync_NotGuid_ReturnsRoleIdIsNotGuid(string roleId)
        {
            // Arrange            
            var expectedMessage = "Role Id is not Guid";

            // Act
            var result = await _roleRequirementsService.GetRoleWithRequirementsAsync(roleId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task GetRoleWithRequirementsAsync_RoleNotExist_ReturnsRoleDoesNotExist()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var expectedMessage = $"Role with Id {roleId} does not exist";

            _rolesRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync((Role)null!);

            // Act
            var result = await _roleRequirementsService.GetRoleWithRequirementsAsync(roleId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task GetRoleWithRequirementsAsync_CorrectRole_ReturnsRoleWithRequirements()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var roleWithoutRequirements = _fixture.Build<Role>()
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .Create();
            var requirements = _fixture.Build<Requirement>()
                .OmitAutoProperties()
                .CreateMany(5)
                .ToList();

            _rolesRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(roleWithoutRequirements);
            _roleRequirementsRepository.Setup(x => x.GetRequirementsOfRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(requirements);

            // Act
            var result = await _roleRequirementsService.GetRoleWithRequirementsAsync(roleId);

            // Assert
            Assert.NotNull(result.RoleWithRequirements);
            Assert.NotNull(result.RoleWithRequirements.Requirements);
            Assert.NotEmpty(result.RoleWithRequirements.Requirements);
            Assert.Equal(5, result.RoleWithRequirements.Requirements.Count);
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("abcd")]
        [InlineData("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxxx")]
        public async Task UpdateRoleWithRequirementsAsync_NotGuid_ReturnsRoleIdIsNotGuid(string roleId)
        {
            // Arrange            
            var expectedMessage = "Role Id is not Guid";

            // Act
            var result = await _roleRequirementsService
                .UpdateRoleWithRequirementsAsync(roleId, null!);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateRoleWithRequirementsAsync_NullListOfRequirements_ReturnsInvalidListOfRequirementsIds()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var expectedMessage = "Invalid list of requirements Ids";

            // Act
            var result = await _roleRequirementsService
                .UpdateRoleWithRequirementsAsync(roleId, null!);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateRoleWithRequirementsAsync_NoRequirementsInDb_ReturnsInvalidListOfRequirementsIds()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var requirementIds = _fixture              
                .CreateMany<Guid>(5)
                .ToList();
            var expectedMessage = "Invalid list of requirements Ids";

            _requirementsRepository.Setup(x => 
                x.CheckEntriesAreExistInDatabase(It.IsAny<List<Guid>>()))
                .Returns(false);

            // Act
            var result = await _roleRequirementsService
                .UpdateRoleWithRequirementsAsync(roleId, requirementIds);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateRoleWithRequirementsAsync_NullSelectedRole_ReturnsRoleDoesNotExist()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var requirementIds = _fixture               
                .CreateMany<Guid>(5)
                .ToList();
            var expectedMessage = $"Role with Id {roleId} does not exist";

            _requirementsRepository.Setup(x => 
                x.CheckEntriesAreExistInDatabase(It.IsAny<List<Guid>>()))
                .Returns(true);
            _rolesRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync((Role)null!);

            // Act
            var result = await _roleRequirementsService
                .UpdateRoleWithRequirementsAsync(roleId, requirementIds);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateRoleWithRequirementsAsync_RoleRequirementsUnsuccessfullyDeleted_ReturnsInnerServerError()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var requirementIds = _fixture
                .CreateMany<Guid>(5)
                .ToList();
            var selectedRole = _fixture.Build<Role>()
                .With(x => x.Id, roleId)
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .Create();
            var expectedMessage = "Inner server error";

            _requirementsRepository.Setup(x => 
                x.CheckEntriesAreExistInDatabase(It.IsAny<List<Guid>>()))
                .Returns(true);
            _rolesRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(selectedRole);
            _roleRequirementsRepository.Setup(x => 
                x.DeleteRoleRequirementsByRoleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _roleRequirementsService
                .UpdateRoleWithRequirementsAsync(roleId, requirementIds);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateRoleWithRequirementsAsync_RoleRequirementsUnsuccessfullyAdded_ReturnsInnerServerError()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var requirementIds = _fixture
                .CreateMany<Guid>(5)
                .ToList();
            var selectedRole = _fixture.Build<Role>()
                .With(x => x.Id, roleId)
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .Create();
            var expectedMessage = "Inner server error";

            _requirementsRepository.Setup(x =>
                x.CheckEntriesAreExistInDatabase(It.IsAny<List<Guid>>()))
                .Returns(true);
            _rolesRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(selectedRole);
            _roleRequirementsRepository.Setup(x =>
                x.DeleteRoleRequirementsByRoleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _roleRequirementsRepository.Setup(x =>
            x.AddRoleRequirementsAsync(It.IsAny<List<RoleRequirement>>()))
                .ReturnsAsync(false);

            // Act
            var result = await _roleRequirementsService
                .UpdateRoleWithRequirementsAsync(roleId, requirementIds);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task UpdateRoleWithRequirementsAsync_CorrectRequest_ReturnsGetRoleWithRequirementsResponse()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();
            var requirementIds = _fixture
                .CreateMany<Guid>(5)
                .ToList();
            var selectedRole = _fixture.Build<Role>()
                .With(x => x.Id, roleId)
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .Create();
            var requirements = _fixture.Build<Requirement>()
                .OmitAutoProperties()
                .CreateMany(5)
                .ToList();

            _requirementsRepository.Setup(x =>
                x.CheckEntriesAreExistInDatabase(It.IsAny<List<Guid>>()))
                .Returns(true);
            _rolesRepository.Setup(x => x.GetRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(selectedRole);
            _roleRequirementsRepository.Setup(x =>
                x.DeleteRoleRequirementsByRoleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _roleRequirementsRepository.Setup(x =>
                x.AddRoleRequirementsAsync(It.IsAny<List<RoleRequirement>>()))
                .ReturnsAsync(true);
            _roleRequirementsRepository.Setup(x =>
                x.GetRequirementsOfRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(requirements);

            // Act
            var result = await _roleRequirementsService
                .UpdateRoleWithRequirementsAsync(roleId, requirementIds);

            // Assert
            Assert.NotNull(result.RoleWithRequirements);
            Assert.NotNull(result.RoleWithRequirements.Requirements);
            Assert.NotEmpty(result.RoleWithRequirements.Requirements);
            Assert.Equal(5, result.RoleWithRequirements.Requirements.Count);
        }

        [Fact]
        public async Task DeleteRoleRequirementsByRequirementIdAsync_RoleRequirementsUnsuccessfullyDeleted_ReturnsFalse()
        {
            // Arrange            
            var requirementId = Guid.NewGuid();
            
            _roleRequirementsRepository.Setup(x =>
                x.DeleteRoleRequirementsByRequirementIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);            

            // Act
            var result = await _roleRequirementsService
                .DeleteRoleRequirementsByRequirementIdAsync(requirementId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteRoleRequirementsByRequirementIdAsync_CorrectRequest_ReturnsTrue()
        {
            // Arrange            
            var requirementId = Guid.NewGuid();

            _roleRequirementsRepository.Setup(x =>
                x.DeleteRoleRequirementsByRequirementIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            var result = await _roleRequirementsService
                .DeleteRoleRequirementsByRequirementIdAsync(requirementId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteRoleRequirementsByRoleIdAsync_RoleRequirementsUnsuccessfullyDeleted_ReturnsFalse()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();

            _roleRequirementsRepository.Setup(x =>
                x.DeleteRoleRequirementsByRoleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _roleRequirementsService
                .DeleteRoleRequirementsByRoleIdAsync(roleId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteRoleRequirementsByRoleIdAsync_CorrectRequest_ReturnsTrue()
        {
            // Arrange            
            var roleId = Guid.NewGuid().ToString();

            _roleRequirementsRepository.Setup(x =>
                x.DeleteRoleRequirementsByRoleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _roleRequirementsService
                .DeleteRoleRequirementsByRoleIdAsync(roleId);

            // Assert
            Assert.True(result);
        }
    }
}
