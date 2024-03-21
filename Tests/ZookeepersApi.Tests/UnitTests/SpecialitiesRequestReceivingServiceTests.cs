using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.ZookeepersApi.Services;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public class SpecialitiesRequestReceivingServiceTests
    {
        private Mock<IResponsesReceiverFromRabbitMq> _mockReceiver;
        private Mock<ISpecialitiesService> _mockSpecialitiesService;
        private Mock<IConnectionService> _mockConnection;

        public SpecialitiesRequestReceivingServiceTests()
        {
            _mockReceiver = new Mock<IResponsesReceiverFromRabbitMq>();
            _mockSpecialitiesService = new Mock<ISpecialitiesService>();
            _mockConnection = new Mock<IConnectionService>();
        }

        [Fact]
        public async void GetAllSpecialities_should_return_error_message()
        {
            var expectedMessage = "Error message from GetResponseFromRabbitTask()";

            var specialitiesResponse = new Fixture().Build<GetAnimalTypesResponse>()
                .With(s => s.AnimalTypes, (List<AnimalType>)null)
                .With(s => s.ErrorMessage, expectedMessage)
                .Create();

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetAllAnimalTypesRequest, GetAnimalTypesResponse>(
                It.IsAny<GetAllAnimalTypesRequest>(), It.IsAny<Uri>())).ReturnsAsync(specialitiesResponse);

            var service = new SpecialitiesRequestReceivingService(_mockSpecialitiesService.Object, _mockReceiver.Object,
                _mockConnection.Object);

            var result = await service.GetAllSpecialities();

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void GetAllSpecialities_should_return_list_of_specialities()
        {
            var allAnimalTypes = new List<AnimalType>()
            {
                new Fixture().Build<AnimalType>().With(at => at.Animals, (List<Animal>)null).Create(),
                new Fixture().Build<AnimalType>().With(at => at.Animals, (List<Animal>)null).Create(),
                new Fixture().Build<AnimalType>().With(at => at.Animals, (List<Animal>)null).Create(),
                new Fixture().Build<AnimalType>().With(at => at.Animals, (List<Animal>)null).Create()
            };
            var specialitiesResponse = new Fixture().Build<GetAnimalTypesResponse>()
                .With(s => s.AnimalTypes, allAnimalTypes)
                .Create();

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetAllAnimalTypesRequest, GetAnimalTypesResponse>(
                It.IsAny<GetAllAnimalTypesRequest>(), It.IsAny<Uri>())).ReturnsAsync(specialitiesResponse);

            var service = new SpecialitiesRequestReceivingService(_mockSpecialitiesService.Object, _mockReceiver.Object,
                _mockConnection.Object);

            var result = await service.GetAllSpecialities();

            Assert.Equal(allAnimalTypes, result.AnimalTypes);
        }
    }
}
