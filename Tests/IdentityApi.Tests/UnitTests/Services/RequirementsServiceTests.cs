using AutoFixture;
using AutoFixture.AutoMoq;
using MicroZoo.IdentityApi.Repositories;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Roles;
using Moq;

namespace MicroZoo.IdentityApi.Tests.UnitTests.Services
{
    public class RequirementsServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IRequirementsRepository> _requirementRepository;
        private readonly Mock<IRoleRequirementsService> _roleRequirementService;
        private readonly RequirementsService _requirementsService;

        public RequirementsServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _requirementRepository = _fixture.Freeze<Mock<IRequirementsRepository>>();
            _roleRequirementService = _fixture.Freeze<Mock<IRoleRequirementsService>>();

            _requirementsService = _fixture.Create<RequirementsService>();
        }

        [Fact]
        public async Task GetAllRequirementsAsync_ReturnsAllRequirements()
        {
            // Arrange
            var requirements = _fixture.Build<Requirement>()
                .OmitAutoProperties()
                .CreateMany(5)
                .ToList();

            _requirementRepository.Setup(x => x.GetAllRequirementsAsync()).ReturnsAsync(requirements);

            // Act
            var result = await _requirementsService.GetAllRequirementsAsync();

            // Assert
            Assert.NotNull(result.Requirements!);
            Assert.NotEmpty(result.Requirements!);
            Assert.Equal(5, result.Requirements!.Count);
        }

        [Fact]
        public async Task GetRequirementAsync_ReturnsRequirementDoesNotExist()
        {
            // Arrange
            var requirementId = Guid.NewGuid();
            var expectedMessage = $"Requirement with Id {requirementId} does not exist";

            _requirementRepository.Setup(x => x.GetRequirementAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Requirement)null!);
            
            // Act
            var result = await _requirementsService.GetRequirementAsync(requirementId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task GetRequirementAsync_ReturnsRequirement()
        {
            // Arrange
            var requirementId = Guid.NewGuid();
            var requirement = _fixture.Build<Requirement>()
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .Create();
            var expectedMessage = $"Requirement with Id {requirementId} does not exist";

            _requirementRepository.Setup(x => x.GetRequirementAsync(It.IsAny<Guid>()))
                .ReturnsAsync(requirement);            

            // Act
            var result = await _requirementsService.GetRequirementAsync(requirementId);

            // Assert
            Assert.NotNull(result.Requirement);
        }

        [Fact]
        public async Task AddRequirementAsync_NullRequirementDto_ReturnsNewRequirementMustBeNotNull()
        {
            // Arrange            
            var expectedMessage = "New requirement must be not null";

            // Act
            var result = await _requirementsService.AddRequirementAsync(null!);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Theory]
        [InlineData((string)null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task AddRequirementAsync_NullOrEmptyNameOfRequirementDto_ReturnsNameOfNewRequirementMustBeNotNullOrEmpty(string name)
        {
            // Arrange            
            var requirementDto = _fixture.Build<RequirementWithoutIdDto>()
                .With(x => x.Name, name)
                .Create();
            var expectedMessage = "Name of new requirement must be not null or empty";

            // Act
            var result = await _requirementsService.AddRequirementAsync(requirementDto);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task AddRequirementAsync_DuplicateRequirementDto_ReturnsRequirementAlreadyExist()
        {
            // Arrange            
            var requirementDto = _fixture.Build<RequirementWithoutIdDto>()
                .Create();            
            var expectedMessage = $"Requirement with name {requirementDto.Name} already exist";

            _requirementRepository.Setup(x => x.AddRequirementAsync(It.IsAny<Requirement>()))
                .ReturnsAsync((Requirement)null!);

            // Act
            var result = await _requirementsService.AddRequirementAsync(requirementDto);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task AddRequirementAsync_ReturnsRequirement()
        {
            // Arrange            
            var requirementDto = _fixture.Build<RequirementWithoutIdDto>()
                .Create();
            var requirement = _fixture.Build<Requirement>()
                .With(x => x.Name, requirementDto.Name)
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .Create();

            _requirementRepository.Setup(x => x.AddRequirementAsync(It.IsAny<Requirement>()))
                .ReturnsAsync(requirement);

            // Act
            var result = await _requirementsService.AddRequirementAsync(requirementDto);

            // Assert
            Assert.NotNull(result.Requirement);
        }

        [Fact]
        public async Task SoftDeleteRequirementAsync_NotExistedGuid_ReturnsRequirementDoesNotExist()
        {
            // Arrange            
            var requirementId = Guid.NewGuid();
            var expectedMessage = $"Requirement with Id {requirementId} does not exist";
            
            _requirementRepository.Setup(x => x.GetRequirementAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Requirement)null!);

            // Act
            var result = await _requirementsService.SoftDeleteRequirementAsync(requirementId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task SoftDeleteRequirementAsync_BadWorkOfRoleRequirementsDeleteon_ReturnsSomethingGoesWrong()
        {
            // Arrange            
            var requirementId = Guid.NewGuid();
            var requirement = _fixture.Build<Requirement>()
                .With(x => x.Id, requirementId)
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .Create();

            var expectedMessage = $"Something goes wrong during delete RoleRequirements" +
                    $"for requirement with Id {requirementId}";

            _requirementRepository.Setup(x => x.GetRequirementAsync(It.IsAny<Guid>()))
                .ReturnsAsync(requirement);
            _roleRequirementService.Setup(x =>
                x.DeleteRoleRequirementsByRequirementIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            var result = await _requirementsService.SoftDeleteRequirementAsync(requirementId);

            // Assert
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async Task SoftDeleteRequirementAsync_CorrectGuid_ReturnsRequirement()
        {
            // Arrange            
            var requirementId = Guid.NewGuid();
            var requirement = _fixture.Build<Requirement>()                
                .With(x => x.Id, requirementId)
                .With(x => x.RoleRequirements, (List<RoleRequirement>)null!)
                .Create();

            var expectedMessage = $"Requirement with Id {requirementId} does not exist";

            _requirementRepository.Setup(x => x.GetRequirementAsync(It.IsAny<Guid>()))
                .ReturnsAsync(requirement);
            _roleRequirementService.Setup(x =>
                x.DeleteRoleRequirementsByRequirementIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);
            _requirementRepository.Setup(x => x.SoftDeleteRequirementAsync(It.IsAny<Requirement>()))
                .ReturnsAsync(requirement);

            // Act
            var result = await _requirementsService.SoftDeleteRequirementAsync(requirementId);

            // Assert
            Assert.NotNull(result.Requirement);
        }
    }
}
