

using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Repository;
using MicroZoo.ZookeepersApi.Services;
using Moq;

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
        public async void GetZookepeerInfoAsync_should_return_full_zookeeper_info()
        {
            // Arrange            
            int id = 1;
            var mock = new Mock<IZookeeperRepository>();
            mock.Setup(l => l.GetPersonByIdFromPersonsApiAsync(It.IsAny<string>())).ReturnsAsync(persons[id]);
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
            Assert.NotNull(result.Adout);
            Assert.NotNull(result.Jobs);
            Assert.NotNull(result.Specialities);
            Assert.NotNull(result.ObservedAnimals);
        }

        [Fact]
        public async void GetZookepeerInfoAsync_should_return_null()
        {
            // Arrange            
            int id = 1;
            var mock = new Mock<IZookeeperRepository>();
            var sut = new ZookeeperApiService(mock.Object);

            // Act
            var result = await sut.GetZookepeerInfoAsync(id);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void GetZookepeerInfoAsync_should_return_only_person_info()
        {
            // Arrange            
            int id = 1;
            var mock = new Mock<IZookeeperRepository>();
            mock.Setup(l => l.GetPersonByIdFromPersonsApiAsync(It.IsAny<string>())).ReturnsAsync(persons[id]);
            var sut = new ZookeeperApiService(mock.Object);

            // Act
            var result = await sut.GetZookepeerInfoAsync(id);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Adout);
            Assert.Null(result.Jobs);
            Assert.Null(result.Specialities);
            Assert.Null(result.ObservedAnimals);
        }

        [Fact]
        public async void GetZookepeerInfoAsync_should_return_person_and_jobs_info()
        {
            // Arrange            
            int id = 1;
            var mock = new Mock<IZookeeperRepository>();
            mock.Setup(l => l.GetPersonByIdFromPersonsApiAsync(It.IsAny<string>())).ReturnsAsync(persons[id]);
            mock.Setup(l => l.GetJobsById(It.IsAny<int>())).ReturnsAsync(jobs);
            var sut = new ZookeeperApiService(mock.Object);

            // Act
            var result = await sut.GetZookepeerInfoAsync(id);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Adout);
            Assert.NotNull(result.Jobs);
            Assert.Null(result.Specialities);
            Assert.Null(result.ObservedAnimals);
        }

        [Fact]
        public async void GetZookepeerInfoAsync_should_return_person_and_jobs_info_without_other_info()
        {
            // Arrange            
            int id = 1;
            var mock = new Mock<IZookeeperRepository>();
            mock.Setup(l => l.GetPersonByIdFromPersonsApiAsync(It.IsAny<string>())).ReturnsAsync(persons[id]);
            mock.Setup(l => l.GetJobsById(It.IsAny<int>())).ReturnsAsync(jobs);
            mock.Setup(l => l.GetSpecialitiesIdByPersonId(It.IsAny<int>())).ReturnsAsync(new List<int>());
            var sut = new ZookeeperApiService(mock.Object);

            // Act
            var result = await sut.GetZookepeerInfoAsync(id);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Adout);
            Assert.NotNull(result.Jobs);
            Assert.Null(result.Specialities);
            Assert.Null(result.ObservedAnimals);
        }

        [Fact]
        public async void GetZookepeerInfoAsync_with_null_specialities_should_return_person_and_jobs_info()
        {
            // Arrange            
            int id = 1;
            var mock = new Mock<IZookeeperRepository>();
            mock.Setup(l => l.GetPersonByIdFromPersonsApiAsync(It.IsAny<string>())).ReturnsAsync(persons[id]);
            mock.Setup(l => l.GetJobsById(It.IsAny<int>())).ReturnsAsync(jobs);
            mock.Setup(l => l.GetSpecialitiesIdByPersonId(It.IsAny<int>())).ReturnsAsync(new List<int>() { 0, 2 });
            var sut = new ZookeeperApiService(mock.Object);

            // Act
            var result = await sut.GetZookepeerInfoAsync(id);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Adout);
            Assert.NotNull(result.Jobs);
            Assert.Null(result.Specialities);
            Assert.Null(result.ObservedAnimals);
        }

        [Fact]
        public async void GetZookepeerInfoAsync_should_return_person_jobs_and_specialities_info()
        {
            // Arrange            
            int id = 1;
            var mock = new Mock<IZookeeperRepository>();
            mock.Setup(l => l.GetPersonByIdFromPersonsApiAsync(It.IsAny<string>())).ReturnsAsync(persons[id]);
            mock.Setup(l => l.GetJobsById(It.IsAny<int>())).ReturnsAsync(jobs);
            mock.Setup(l => l.GetSpecialitiesIdByPersonId(It.IsAny<int>())).ReturnsAsync(new List<int>() { 0, 2 });
            mock.Setup(l => l.GetAnimalTypesByIds(It.IsAny<string>())).ReturnsAsync(animalTypes);
            var sut = new ZookeeperApiService(mock.Object);

            // Act
            var result = await sut.GetZookepeerInfoAsync(id);

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Adout);
            Assert.NotNull(result.Jobs);
            Assert.NotNull(result.Specialities);
            Assert.Null(result.ObservedAnimals);
        }

        [Fact]
        public async void GetPersonByIdFromPersonsApiAsync_should_return_person()
        {
            // Arrange            
            int id = 1;
            var mock = new Mock<IZookeeperRepository>();
            mock.Setup(l => l.GetPersonByIdFromPersonsApiAsync(It.IsAny<string>())).ReturnsAsync(persons[id]);
            var sut = new ZookeeperApiService(mock.Object);

            // Act
            var result = await sut.GetPersonByIdFromPersonsApiAsync(id);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void GetPersonByIdFromPersonsApiAsync_should_return_null()
        {
            // Arrange            
            int id = 999999999;
            var mock = new Mock<IZookeeperRepository>();
            var sut = new ZookeeperApiService(mock.Object);

            // Act
            var result = await sut.GetPersonByIdFromPersonsApiAsync(id);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void GetAllAnimalTypesFromAnimalsApiAsync_should_return_animalTypes()
        {
            // Arrange   
            var mock = new Mock<IZookeeperRepository>();
            mock.Setup(l => l.GetAllAnimalTypesFromAnimalsApiAsync(It.IsAny<string>())).ReturnsAsync(animalTypes);
            var sut = new ZookeeperApiService(mock.Object);

            // Act
            var result = await sut.GetAllAnimalTypesFromAnimalsApiAsync();

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void GetAllAnimalTypesFromAnimalsApiAsync_should_return_null()
        {
            // Arrange            
            var mock = new Mock<IZookeeperRepository>();
            //mock.Setup(l => l.GetPersonByIdFromPersonsApiAsync(It.IsAny<string>())).ReturnsAsync(persons[id]);
            var sut = new ZookeeperApiService(mock.Object);

            // Act
            var result = await sut.GetAllAnimalTypesFromAnimalsApiAsync();

            //Assert
            Assert.Null(result);
        }
    }
}
