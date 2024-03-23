using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Specialities.Dto;
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

        [Fact]
        public async void AddSpeciality_and_person_is_null_should_return_error_message_person_is_null()
        {
            var personErrorMessage = "Error. Person is null";
            var expectedMessage = personErrorMessage + ".\n";
            var specialityDto = new Fixture().Create<SpecialityDto>();

            var personResponse = new Fixture().Build<GetPersonResponse>()
                .With(r => r.Person, (Person)null)
                .With(r => r.ErrorMessage, personErrorMessage)
                .Create();

            var animalType = new Fixture().Build<AnimalType>()
                .With(at => at.Animals, (List<Animal>)null)
                .With(at => at.Id, specialityDto.AnimalTypeId)
                .Create(); 

            var animalTypeResponse = new Fixture().Build<GetAnimalTypeResponse>()
                .With(atr => atr.AnimalType, animalType)
                .Create();

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetAnimalTypeRequest, GetAnimalTypeResponse>(
                It.IsAny<GetAnimalTypeRequest>(), It.IsAny<Uri>())).ReturnsAsync(animalTypeResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<AddSpecialityRequest, GetSpecialityResponse>(
                It.IsAny<AddSpecialityRequest>(), It.IsAny<Uri>()));

            var service = new SpecialitiesRequestReceivingService(_mockSpecialitiesService.Object, _mockReceiver.Object,
                _mockConnection.Object);

            var result = await service.AddSpeciality(specialityDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void AddSpeciality_and_animalType_is_null_should_return_error_message_animalType_is_null()
        {
            var expectedMessage = "Error. AnimalType is null";
            var specialityDto = new Fixture().Create<SpecialityDto>();

            var personResponse = new Fixture().Create<GetPersonResponse>();

            var animalTypeResponse = new Fixture().Build<GetAnimalTypeResponse>()
                .With(atr => atr.AnimalType, (AnimalType)null)
                .With(r => r.ErrorMessage, expectedMessage)
                .Create();

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetAnimalTypeRequest, GetAnimalTypeResponse>(
                It.IsAny<GetAnimalTypeRequest>(), It.IsAny<Uri>())).ReturnsAsync(animalTypeResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<AddSpecialityRequest, GetSpecialityResponse>(
                It.IsAny<AddSpecialityRequest>(), It.IsAny<Uri>()));

            var service = new SpecialitiesRequestReceivingService(_mockSpecialitiesService.Object, _mockReceiver.Object,
                _mockConnection.Object);

            var result = await service.AddSpeciality(specialityDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void AddSpeciality_and_person_and_animalType_is_null_should_return_error_message_person_is_null_animalType_is_null()
        {
            var personErrorMessage = "Error. Person is null";
            var animalTypeErrorMessage = "Error. AnimalType is null";
            var expectedMessage = personErrorMessage + ".\n" + animalTypeErrorMessage;
            var specialityDto = new Fixture().Create<SpecialityDto>();

            var personResponse = new Fixture().Build<GetPersonResponse>()
                .With(r => r.Person, (Person)null)
                .With(r => r.ErrorMessage, personErrorMessage)
                .Create();

            var animalTypeResponse = new Fixture().Build<GetAnimalTypeResponse>()
                .With(atr => atr.AnimalType, (AnimalType)null)
                .With(r => r.ErrorMessage, animalTypeErrorMessage)
                .Create();

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetAnimalTypeRequest, GetAnimalTypeResponse>(
                It.IsAny<GetAnimalTypeRequest>(), It.IsAny<Uri>())).ReturnsAsync(animalTypeResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<AddSpecialityRequest, GetSpecialityResponse>(
                It.IsAny<AddSpecialityRequest>(), It.IsAny<Uri>()));

            var service = new SpecialitiesRequestReceivingService(_mockSpecialitiesService.Object, _mockReceiver.Object,
                _mockConnection.Object);

            var result = await service.AddSpeciality(specialityDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void AddSpeciality_should_return_speciality()
        {
            var specialityDto = new Fixture().Create<SpecialityDto>();

            var personResponse = new Fixture().Create<GetPersonResponse>();

            var animalType = new Fixture().Build<AnimalType>()
                .With(at => at.Animals, (List<Animal>)null)
                .With(at => at.Id, specialityDto.AnimalTypeId)
                .Create();

            var animalTypeResponse = new Fixture().Build<GetAnimalTypeResponse>()
                .With(atr => atr.AnimalType, animalType)
                .Create();

            var specialityResponse = new Fixture().Create<GetSpecialityResponse>();

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetAnimalTypeRequest, GetAnimalTypeResponse>(
                It.IsAny<GetAnimalTypeRequest>(), It.IsAny<Uri>())).ReturnsAsync(animalTypeResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<AddSpecialityRequest, GetSpecialityResponse>(
                It.IsAny<AddSpecialityRequest>(), It.IsAny<Uri>())).ReturnsAsync(specialityResponse);

            var service = new SpecialitiesRequestReceivingService(_mockSpecialitiesService.Object, _mockReceiver.Object,
                _mockConnection.Object);

            var result = await service.AddSpeciality(specialityDto);

            Assert.Equal(specialityResponse.Speciality, result.Speciality);
        }

        [Fact]
        public async void ChangeRelationBetweenZookeeperAndSpeciality_and_person_is_null_should_return_error_message_person_is_null()
        {
            var personErrorMessage = "Error. Person is null";
            var expectedMessage = personErrorMessage + ".\n";

            var relationId = new Fixture().Create<int>();
            var specialityDto = new Fixture().Create<SpecialityDto>();

            var personResponse = new Fixture().Build<GetPersonResponse>()
                .With(r => r.Person, (Person)null)
                .With(r => r.ErrorMessage, personErrorMessage)
                .Create();

            var animalType = new Fixture().Build<AnimalType>()
                .With(at => at.Animals, (List<Animal>)null)
                .With(at => at.Id, specialityDto.AnimalTypeId)
                .Create();

            var animalTypeResponse = new Fixture().Build<GetAnimalTypeResponse>()
                .With(atr => atr.AnimalType, animalType)
                .Create();

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetAnimalTypeRequest, GetAnimalTypeResponse>(
                It.IsAny<GetAnimalTypeRequest>(), It.IsAny<Uri>())).ReturnsAsync(animalTypeResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<ChangeRelationBetweenZookeeperAndSpecialityRequest,
                GetSpecialityResponse>(It.IsAny<ChangeRelationBetweenZookeeperAndSpecialityRequest>(), It.IsAny<Uri>()));

            var service = new SpecialitiesRequestReceivingService(_mockSpecialitiesService.Object, _mockReceiver.Object,
                _mockConnection.Object);

            var result = await service.ChangeRelationBetweenZookeeperAndSpeciality(relationId, specialityDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void ChangeRelationBetweenZookeeperAndSpeciality_and_animalType_is_null_should_return_error_message_animalType_is_null()
        {            
            var expectedMessage = "Error. AnimalType is null";

            var relationId = new Fixture().Create<int>();
            var specialityDto = new Fixture().Create<SpecialityDto>();

            var personResponse = new Fixture().Create<GetPersonResponse>();

            var animalTypeResponse = new Fixture().Build<GetAnimalTypeResponse>()
                .With(atr => atr.AnimalType, (AnimalType)null)
                .With(r => r.ErrorMessage, expectedMessage)
                .Create();

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetAnimalTypeRequest, GetAnimalTypeResponse>(
                It.IsAny<GetAnimalTypeRequest>(), It.IsAny<Uri>())).ReturnsAsync(animalTypeResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<ChangeRelationBetweenZookeeperAndSpecialityRequest,
                GetSpecialityResponse>(It.IsAny<ChangeRelationBetweenZookeeperAndSpecialityRequest>(), It.IsAny<Uri>()));

            var service = new SpecialitiesRequestReceivingService(_mockSpecialitiesService.Object, _mockReceiver.Object,
                _mockConnection.Object);

            var result = await service.ChangeRelationBetweenZookeeperAndSpeciality(relationId, specialityDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void ChangeRelationBetweenZookeeperAndSpeciality_and_person_and_animalType_is_null_should_return_error_message_person_is_null_animalType_is_null()
        {
            var personErrorMessage = "Error. Person is null";
            var animalTypeErrorMessage = "Error. AnimalType is null";
            var expectedMessage = personErrorMessage + ".\n" + animalTypeErrorMessage;

            var relationId = new Fixture().Create<int>();
            var specialityDto = new Fixture().Create<SpecialityDto>();

            var personResponse = new Fixture().Build<GetPersonResponse>()
                .With(r => r.Person, (Person)null)
                .With(r => r.ErrorMessage, personErrorMessage)
                .Create();

            var animalTypeResponse = new Fixture().Build<GetAnimalTypeResponse>()
                .With(atr => atr.AnimalType, (AnimalType)null)
                .With(r => r.ErrorMessage, animalTypeErrorMessage)
                .Create();

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetAnimalTypeRequest, GetAnimalTypeResponse>(
                It.IsAny<GetAnimalTypeRequest>(), It.IsAny<Uri>())).ReturnsAsync(animalTypeResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<ChangeRelationBetweenZookeeperAndSpecialityRequest,
                GetSpecialityResponse>(It.IsAny<ChangeRelationBetweenZookeeperAndSpecialityRequest>(), It.IsAny<Uri>()));

            var service = new SpecialitiesRequestReceivingService(_mockSpecialitiesService.Object, _mockReceiver.Object,
                _mockConnection.Object);

            var result = await service.ChangeRelationBetweenZookeeperAndSpeciality(relationId, specialityDto);

            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void ChangeRelationBetweenZookeeperAndSpeciality_speciality()
        {
            var relationId = new Fixture().Create<int>();
            var specialityDto = new Fixture().Create<SpecialityDto>();

            var personResponse = new Fixture().Create<GetPersonResponse>();

            var animalType = new Fixture().Build<AnimalType>()
                .With(at => at.Animals, (List<Animal>)null)
                .With(at => at.Id, specialityDto.AnimalTypeId)
                .Create();

            var animalTypeResponse = new Fixture().Build<GetAnimalTypeResponse>()
                .With(atr => atr.AnimalType, animalType)
                .Create();

            var specialityResponse = new Fixture().Create<GetSpecialityResponse>();

            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                It.IsAny<GetPersonRequest>(), It.IsAny<Uri>())).ReturnsAsync(personResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<GetAnimalTypeRequest, GetAnimalTypeResponse>(
                It.IsAny<GetAnimalTypeRequest>(), It.IsAny<Uri>())).ReturnsAsync(animalTypeResponse);
            _mockReceiver.Setup(s => s.GetResponseFromRabbitTask<ChangeRelationBetweenZookeeperAndSpecialityRequest,
                GetSpecialityResponse>(It.IsAny<ChangeRelationBetweenZookeeperAndSpecialityRequest>(), It.IsAny<Uri>())).
                ReturnsAsync(specialityResponse);

            var service = new SpecialitiesRequestReceivingService(_mockSpecialitiesService.Object, _mockReceiver.Object,
                _mockConnection.Object);

            var result = await service.ChangeRelationBetweenZookeeperAndSpeciality(relationId, specialityDto);

            Assert.Equal(specialityResponse.Speciality, result.Speciality);
        }
    }
}
