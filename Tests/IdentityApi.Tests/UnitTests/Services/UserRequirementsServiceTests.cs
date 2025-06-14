using AutoFixture;
using AutoFixture.AutoMoq;
using MicroZoo.IdentityApi.Repositories;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Users;
using Moq;

namespace MicroZoo.IdentityApi.Tests.UnitTests.Services
{
    public class UserRequirementsServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUserRequirementsRepository> _userRequirementsRepository;
        private readonly UserRequirementsService _userRequirementsService;

        public UserRequirementsServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _userRequirementsRepository = _fixture.Freeze<Mock<IUserRequirementsRepository>>();

            _userRequirementsService = _fixture.Create<UserRequirementsService>();
        }

        [Fact]
        public async Task GetAllowedRequirementsOfUser_CorrectData_ReturnsAllowedRequirementsOfUser()
        {
            // Arrange
            var user = _fixture.Build<User>().Create();
            var requirements = _fixture.Build<string>()
                .CreateMany(5)
                .ToList();

            _userRequirementsRepository.Setup(x => x.GetAllowedRequirementsOfUser(It.IsAny<User>()))
                .ReturnsAsync(requirements);

            // Act
            var result = await _userRequirementsService.GetAllowedRequirementsOfUser(user);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(5, result.Count);
        }
    }
}
