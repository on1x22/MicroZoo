

using Microsoft.AspNetCore.Http.HttpResults;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.ZookeepersApi.Models;
using MicroZoo.ZookeepersApi.Repository;
using MicroZoo.ZookeepersApi.Apis;

namespace MicroZoo.ZookeepersApi.Tests.UnitTests
{
    public class ZookeeperApiTests
    {
        [Fact]
        public async void GetById_should_return_zookeeper()
        {
            /*List<ZookeeperInfo> _zookeeperInfos = new List<ZookeeperInfo>()
            {
                new Fixture().Build<ZookeeperInfo>().Create(),
                new Fixture().Build<ZookeeperInfo>().Create(),
                new Fixture().Build<ZookeeperInfo>().Create(),
            };*/

            List<Person> _persons = new List<Person>()
            {
                new Fixture().Build<Person>().Create(),
                new Fixture().Build<Person>().Create(),
                new Fixture().Build<Person>().Create(),
                new Fixture().Build<Person>().Create(),
                new Fixture().Build<Person>().Create(),
            };

            // Arrange            
            int id = 1;
            /*var mock = new Mock<IZookeeperRepository>();
            mock.Setup(l => l.GetPersonByIdFromPersonsApiAsync(It.IsAny<int>())).ReturnsAsync(_persons[id]);
            var mockRepository = mock.Object;
            string expected = _persons[id].FirstName + " " + _persons[id].LastName;*/

            // Act
            /*var person = await ZookeeperApi.GetById(id, mockRepository);
            var result = (person as Ok<Person>)?.Value;
            string actual = result?.FirstName + " " + result?.LastName;*/

            //Assert
            /*Assert.NotNull(result);
            Assert.Equal(expected, actual);*/
        }

        [Fact]
        public async void GetZookepeerInfoAsync_should_return_zookeeper()
        {

        }
    }
}
