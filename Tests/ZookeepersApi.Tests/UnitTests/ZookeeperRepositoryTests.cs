using Microsoft.AspNetCore.Http.HttpResults;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.ZookeepersApi.Apis;
using MicroZoo.ZookeepersApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZookeepersApi.Tests.UnitTests
{
    public class ZookeeperRepositoryTests
    {
        List<Person> _persons = new List<Person>()
        {
            new Fixture().Build<Person>().Create(),
            new Fixture().Build<Person>().Create(),
            new Fixture().Build<Person>().Create(),
            new Fixture().Build<Person>().Create(),
            new Fixture().Build<Person>().Create(),
        };

        [Fact]
        public void GetZookepeerInfoAsync_should_return_info()
        {
            // Arrange            
            /*int id = 1;
            var mock = new Mock<IZookeeperRepository>();
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
    }
}
