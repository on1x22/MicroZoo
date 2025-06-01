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
            //var requirements = _fixture.CreateMany<Requirement>(5).ToList();
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
    }
}
