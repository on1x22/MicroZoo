

using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Repository;
using MicroZoo.ZookeepersApi.Services;

namespace microZoo.ZookeepersApi.Tests.UnitTests
{
    public class ZookeeperApiServiceTests
    {
        List<Person> persons = new List<Person>()
        {
            new Fixture().Build<Person>().Create(),
            new Fixture().Build<Person>().Create(),
            new Fixture().Build<Person>().Create(),
            new Fixture().Build<Person>().Create(),
            new Fixture().Build<Person>().Create()
        };

        List<Job> jobs = new List<Job>()
        {
            new Fixture().Build<Job>().Create(),
            new Fixture().Build<Job>().Create(),
            new Fixture().Build<Job>().Create()
        };

        List<AnimalType> animalTypes = new List<AnimalType>()
        {
            new Fixture().Build<AnimalType>().Without(p => p.Animals).Create(),
            new Fixture().Build<AnimalType>().Without(p => p.Animals).Create(),
            new Fixture().Build<AnimalType>().Without(p => p.Animals).Create()
        };

        List<Animal> animals = new List<Animal>()
        {
            new Fixture().Build<Animal>().Without(p => p.AnimalType).Create(),
            new Fixture().Build<Animal>().Without(p => p.AnimalType).Create(),
            new Fixture().Build<Animal>().Without(p => p.AnimalType).Create()
        };

        List<ObservedAnimal> ObservedAnimals = new List<ObservedAnimal>()
        {
            new Fixture().Build<ObservedAnimal>().Create(),
            new Fixture().Build<ObservedAnimal>().Create(),
            new Fixture().Build<ObservedAnimal>().Create()
        };

        [Fact]
        public async void GetZookepeerInfoAsync_should_return_zookeeper_info()
        {
            // Arrange            
            int id = 1;
            //var mockService = new Mock<IZookeeperApiService>();
            //mockService.Setup(l => l.GetPersonByIdFromPersonsApiAsync(It.IsAny<int>())).ReturnsAsync(persons[id]);

            var mock = new Mock<IZookeeperRepository>();
            mock.Setup(l => l.GetJobsById(It.IsAny<int>())).ReturnsAsync(jobs);
            mock.Setup(l => l.GetSpecialitiesIdByPersonId(It.IsAny<int>())).ReturnsAsync(new List<int>() { 0, 2 });
            mock.Setup(l => l.GetAnimalTypesByIds(It.IsAny<string>())).ReturnsAsync(animalTypes);
            mock.Setup(l => l.GetAnimalsByAnimalTypesIds(It.IsAny<string>())).ReturnsAsync(animals);
            mock.Setup(l => l.GetObservedAnimals(It.IsAny<List<Animal>>(), It.IsAny<List<AnimalType>>()))
                                                                                .Returns(ObservedAnimals);
            var mockRepository = mock.Object;
            var sut = new ZookeeperApiService(mockRepository);

            // Act
            var result = await sut.GetZookepeerInfoAsync(id);

            //Assert
            Assert.NotNull(result);
        }
    }
}
