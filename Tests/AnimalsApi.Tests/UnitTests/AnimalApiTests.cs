using Microsoft.AspNetCore.Http.HttpResults;
using MicroZoo.AnimalsApi.Apis;
using MicroZoo.AnimalsApi.Repository;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.Tests.UnitTests
{
    public class AnimalApiTests
    {
        List<Animal> allAnimals = new List<Animal>()
        {
            new Fixture().Build<Animal>().Without(p => p.AnimalType).Create(),
            new Fixture().Build<Animal>().Without(p => p.AnimalType).Create(),
            new Fixture().Build<Animal>().Without(p => p.AnimalType).Create(),
            new Fixture().Build<Animal>().Without(p => p.AnimalType).Create(),
            new Fixture().Build<Animal>().Without(p => p.AnimalType).Create()
        };
        List<AnimalType> allTypes = new List<AnimalType>()
        {
            new Fixture().Build<AnimalType>().Without(p => p.Animals).Create(),
            new Fixture().Build<AnimalType>().Without(p => p.Animals).Create(),
            new Fixture().Build<AnimalType>().Without(p => p.Animals).Create()
        };

        [Fact]
        public async void GetAnimalsByTypes_should_return_animals_by_given_ids()
        {
            // Arrange            
            int[] ids = new int[] { 1, 3 };
            var expected = new List<Animal>() { allAnimals[ids[0]], allAnimals[ids[1]] };
            var mock = new Mock<IAnimalRepository>();
            mock.Setup(l => l.GetAnimalsByTypesAsync(It.IsAny<int[]>())).ReturnsAsync(expected);
            var mockRepository = mock.Object;            

            // Act
            var listOfAnimals = await Apis.AnimalsApi.GetAnimalsByTypes2(ids, mockRepository);
            var result = (listOfAnimals as Ok<List<Animal>>)?.Value;

            //Assert
            Assert.IsType<Ok<List<Animal>>>(listOfAnimals);
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public async void GetAnimalsByTypes_should_return_notFound_result()
        {
            // Arrange      
            var mock = new Mock<IAnimalRepository>();
            var mockRepository = mock.Object;

            // Act
            var result = await Apis.AnimalsApi.GetAnimalsByTypes2(null, mockRepository);            

            //Assert
            Assert.IsType<NotFound<string>>(result);            
        }

        [Fact]
        public async void GetAllAnimalTypes_should_return_all_animal_types()
        {
            // Arrange
            var mock = new Mock<IAnimalRepository>();
            mock.Setup(l => l.GetAllAnimalTypesAsync()).ReturnsAsync(allTypes);
            var mockRepository = mock.Object;

            // Act
            var listOfTypes = await Apis.AnimalsApi.GetAllAnimalTypes(mockRepository);
            var result = (listOfTypes as Ok<List<AnimalType>>)?.Value;

            //Assert
            Assert.IsType<Ok<List<AnimalType>>>(listOfTypes);
            Assert.NotNull(result);
            Assert.Equal(allTypes, result);
        }

        [Fact]
        public async void GetAllAnimalTypes_should_return_NoContent_result()
        {
            // Arrange      
            var mock = new Mock<IAnimalRepository>();
            var mockRepository = mock.Object;

            // Act
            var result = await Apis.AnimalsApi.GetAllAnimalTypes(mockRepository);

            //Assert
            Assert.IsType<NoContent>(result);
        }

        [Fact]
        public async void GetAnimalTypesByIds_should_return_animal_types_by_given_ids()
        {
            // Arrange      
            int[] ids = new int[] { 1, 2 };
            var expected = new List<AnimalType>() { allTypes[ids[0]], allTypes[ids[1]] };
            var mock = new Mock<IAnimalRepository>();
            mock.Setup(l => l.GetAnimalTypesByIdsAsync(It.IsAny<int[]>())).ReturnsAsync(expected);
            var mockRepository = mock.Object;

            // Act
            var listOfTypes = await Apis.AnimalsApi.GetAnimalTypesByIds(ids, mockRepository);
            var result = (listOfTypes as Ok<List<AnimalType>>)?.Value;

            //Assert
            Assert.IsType<Ok<List<AnimalType>>>(listOfTypes);
            Assert.NotNull(result);
            Assert.Equal(expected, result);

        }

        [Fact]
        public async void GetAnimalTypesByIds_should_return_NotFound_result()
        {
            // Arrange      
            var mock = new Mock<IAnimalRepository>();
            var mockRepository = mock.Object;

            // Act
            var result = await Apis.AnimalsApi.GetAnimalTypesByIds(null, mockRepository);

            //Assert
            Assert.IsType<NotFound<string>>(result);
        }
    }
}
