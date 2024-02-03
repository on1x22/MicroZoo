using Microsoft.Extensions.Logging;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.Infrastructure.Models.Specialities.Dto;
using MicroZoo.ZookeepersApi.Repository;
using MicroZoo.ZookeepersApi.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public class SpecialitiesServiceTests
    {
        [Fact]
        public async void CheckZokeepersWithSpecialityAreExistAsync_should_return_false_because_animalType_is_not_exist()
        {
            var animalType = CheckType.AnimalType;
            int animalTypeId = new Fixture().Create<int>();

            var mockRepo = new Mock<ISpecialitiesRepository>();
            mockRepo.Setup(s => s.CheckZokeepersWithSpecialityAreExistAsync(It.IsAny<int>())).ReturnsAsync(false);
            mockRepo.Setup(s => s.CheckZookeeperIsExistAsync(It.IsAny<int>())).ReturnsAsync(true);
            var mockLogger = new Mock<ILogger<SpecialitiesService>>();

            var specialitiesService = new SpecialitiesService(mockRepo.Object, mockLogger.Object);

            var result = await specialitiesService.CheckZokeepersWithSpecialityAreExistAsync(animalType, animalTypeId);

            Assert.False(result.IsThereZookeeperWithThisSpeciality);
        }

        [Fact]
        public async void CheckZokeepersWithSpecialityAreExistAsync_should_return_true_because_animalType_is_exist()
        {
            var animalType = CheckType.AnimalType;
            int animalTypeId = new Fixture().Create<int>();

            var mockRepo = new Mock<ISpecialitiesRepository>();
            mockRepo.Setup(s => s.CheckZokeepersWithSpecialityAreExistAsync(It.IsAny<int>())).ReturnsAsync(true);
            mockRepo.Setup(s => s.CheckZookeeperIsExistAsync(It.IsAny<int>())).ReturnsAsync(false);
            var mockLogger = new Mock<ILogger<SpecialitiesService>>();

            var specialitiesService = new SpecialitiesService(mockRepo.Object, mockLogger.Object);

            var result = await specialitiesService.CheckZokeepersWithSpecialityAreExistAsync(animalType, animalTypeId);

            Assert.True(result.IsThereZookeeperWithThisSpeciality);
        }

        [Fact]
        public async void CheckZokeepersWithSpecialityAreExistAsync_should_return_false_because_zookeeper_is_not_exist()
        {
            var zookeeperType = CheckType.Person;
            int zookeeperId = new Fixture().Create<int>();

            var mockRepo = new Mock<ISpecialitiesRepository>();
            mockRepo.Setup(s => s.CheckZokeepersWithSpecialityAreExistAsync(It.IsAny<int>())).ReturnsAsync(true);
            mockRepo.Setup(s => s.CheckZookeeperIsExistAsync(It.IsAny<int>())).ReturnsAsync(false);
            var mockLogger = new Mock<ILogger<SpecialitiesService>>();

            var specialitiesService = new SpecialitiesService(mockRepo.Object, mockLogger.Object);

            var result = await specialitiesService.CheckZokeepersWithSpecialityAreExistAsync(zookeeperType, zookeeperId);

            Assert.False(result.IsThereZookeeperWithThisSpeciality);
        }

        [Fact]
        public async void CheckZokeepersWithSpecialityAreExistAsync_should_return_false_because_zookeeper_is_exist()
        {
            var zookeeperType = CheckType.Person;
            int zookeeperId = new Fixture().Create<int>();

            var mockRepo = new Mock<ISpecialitiesRepository>();
            mockRepo.Setup(s => s.CheckZokeepersWithSpecialityAreExistAsync(It.IsAny<int>())).ReturnsAsync(false);
            mockRepo.Setup(s => s.CheckZookeeperIsExistAsync(It.IsAny<int>())).ReturnsAsync(true);
            var mockLogger = new Mock<ILogger<SpecialitiesService>>();

            var specialitiesService = new SpecialitiesService(mockRepo.Object, mockLogger.Object);

            var result = await specialitiesService.CheckZokeepersWithSpecialityAreExistAsync(zookeeperType, zookeeperId);

            Assert.True(result.IsThereZookeeperWithThisSpeciality);
        }

        [Fact]
        public async void AddSpecialityAsync_should_return_error_message_association_already_exist()
        {
            var expectedMessage = "Association between specified zookeeper and animal type already exist";

            var specialityDto = new Fixture().Build<SpecialityDto>().Create();

            var mockRepo = new Mock<ISpecialitiesRepository>();
            mockRepo.Setup(s => s.AddSpecialityAsync(It.IsAny<SpecialityDto>())).ThrowsAsync(new Exception());
            var mockLogger = new Mock<ILogger<SpecialitiesService>>();

            var specialitiesService = new SpecialitiesService(mockRepo.Object, mockLogger.Object);

            var result = await specialitiesService.AddSpecialityAsync(specialityDto);

            Assert.Null(result.Speciality);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void AddSpecialityAsync_should_return_speciality()
        {
            var specialityDto = new Fixture().Build<SpecialityDto>().Create();

            var speciality = new Fixture().Build<Speciality>()
                .With(s => s.ZookeeperId, specialityDto.ZookeeperId)
                .With(s => s.AnimalTypeId, specialityDto.AnimalTypeId)
                .Create();

            var mockRepo = new Mock<ISpecialitiesRepository>();
            mockRepo.Setup(s => s.AddSpecialityAsync(It.IsAny<SpecialityDto>())).ReturnsAsync(speciality);
            var mockLogger = new Mock<ILogger<SpecialitiesService>>();

            var specialitiesService = new SpecialitiesService(mockRepo.Object, mockLogger.Object);

            var result = await specialitiesService.AddSpecialityAsync(specialityDto);

            Assert.Null(result.ErrorMessage);
            Assert.Equal(speciality, result.Speciality);
        }

        [Fact]
        public async void ChangeRelationBetweenZookeeperAndSpecialityAsync_should_return_error_message_association_already_exist()
        {
            var expectedMessage = "Association between specified zookeeper and animal type already exist";
            
            int relationId = new Fixture().Create<int>();
            var specialityDto = new Fixture().Build<SpecialityDto>().Create();

            var mockRepo = new Mock<ISpecialitiesRepository>();
            mockRepo.Setup(s => s.ChangeRelationBetweenZookeeperAndSpecialityAsync(It.IsAny<int>(), 
                It.IsAny<SpecialityDto>())).ThrowsAsync(new Exception());
            var mockLogger = new Mock<ILogger<SpecialitiesService>>();

            var specialitiesService = new SpecialitiesService(mockRepo.Object, mockLogger.Object);

            var result = await specialitiesService.ChangeRelationBetweenZookeeperAndSpecialityAsync(relationId,
                specialityDto);

            Assert.Null(result.Speciality);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void ChangeRelationBetweenZookeeperAndSpecialityAsync_should_return_speciality()
        {
            int relationId = new Fixture().Create<int>();
            var specialityDto = new Fixture().Build<SpecialityDto>().Create();

            var speciality = new Fixture().Build<Speciality>()
                .With(s => s.ZookeeperId, specialityDto.ZookeeperId)
                .With(s => s.AnimalTypeId, specialityDto.AnimalTypeId)
                .Create();

            var mockRepo = new Mock<ISpecialitiesRepository>();
            mockRepo.Setup(s => s.ChangeRelationBetweenZookeeperAndSpecialityAsync(It.IsAny<int>(),
                It.IsAny<SpecialityDto>())).ReturnsAsync(speciality);
            var mockLogger = new Mock<ILogger<SpecialitiesService>>();

            var specialitiesService = new SpecialitiesService(mockRepo.Object, mockLogger.Object);

            var result = await specialitiesService.ChangeRelationBetweenZookeeperAndSpecialityAsync(relationId,
                specialityDto);

            Assert.Null(result.ErrorMessage);
            Assert.Equal(speciality, result.Speciality);
        }

        [Fact]
        public async void DeleteSpecialityAsync_should_return_error_message_association_is_not_exist()
        {
            //int relationId = new Fixture().Create<int>();
            var specialityDto = new Fixture().Build<SpecialityDto>().Create();

            Speciality? deletedSpeciality = null;
            List<Speciality> specialityList = null;
            
            var expectedMessage = $"Speciealities for zookeeper with id={specialityDto.ZookeeperId} are not exist";

            var mockRepo = new Mock<ISpecialitiesRepository>();
            mockRepo.Setup(s => s.DeleteSpecialityAsync(It.IsAny<SpecialityDto>())).ReturnsAsync(deletedSpeciality);
            mockRepo.Setup(s => s.GetSpecialitiesByZookeeperIdAsync(It.IsAny<int>())).ReturnsAsync(specialityList);
            var mockLogger = new Mock<ILogger<SpecialitiesService>>();

            var specialitiesService = new SpecialitiesService(mockRepo.Object, mockLogger.Object);

            var result = await specialitiesService.DeleteSpecialityAsync(specialityDto);

            Assert.Null(result.Specialities);
            Assert.Equal(expectedMessage, result.ErrorMessage);
        }

        [Fact]
        public async void DeleteSpecialityAsync_should_return_specialities_of_zookeeper()
        {
            //int relationId = new Fixture().Create<int>();
            var specialityDto = new Fixture().Build<SpecialityDto>().Create();

            Speciality deletedSpeciality = new Fixture().Build<Speciality>()
                .With(s => s.ZookeeperId, specialityDto.ZookeeperId)
                .With(s => s.AnimalTypeId, specialityDto.AnimalTypeId)
                .Create();

            List<Speciality> specialityList = new Fixture().Build<List<Speciality>>().Create();

            var mockRepo = new Mock<ISpecialitiesRepository>();
            mockRepo.Setup(s => s.DeleteSpecialityAsync(It.IsAny<SpecialityDto>())).ReturnsAsync(deletedSpeciality);
            mockRepo.Setup(s => s.GetSpecialitiesByZookeeperIdAsync(It.IsAny<int>())).ReturnsAsync(specialityList);
            var mockLogger = new Mock<ILogger<SpecialitiesService>>();

            var specialitiesService = new SpecialitiesService(mockRepo.Object, mockLogger.Object);

            var result = await specialitiesService.DeleteSpecialityAsync(specialityDto);

            Assert.Null(result.ErrorMessage);
            Assert.Equal(specialityList, result.Specialities);
        }
    }
}
