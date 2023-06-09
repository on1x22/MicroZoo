using Microsoft.AspNetCore.Http.HttpResults;
using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.PersonsApi.Repository;
using MicroZoo.PersonsApi.Apis;

namespace MicroZoo.PersonsApi.Tests.UnitTests
{
    public class PersonApiTests
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
        public async void GetById_should_return_person_with_that_id()
        {
            // Arrange            
            int id = 1;
            var mock = new Mock<IPersonRepository>();
            mock.Setup(l => l.GetPersonById(It.IsAny<int>())).ReturnsAsync(_persons[id]);
            var mockRepository = mock.Object;
            string expected = _persons[id].FirstName + " " + _persons[id].LastName;

            // Act
            var person = await PersonApi.GetPersonById(id, mockRepository);    
            var result = (person as Ok<Person>)?.Value;
            string actual = result?.FirstName + " " + result?.LastName;

            //Assert
            Assert.NotNull(result);            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void UpdatePerson_should_update_person_info()
        {            
            // Arrange            
            int id = 1;
            var updatedPerson = new Fixture().Build<Person>().Create();
            var mock = new Mock<IPersonRepository>();
            mock.Setup(l => l.UpdatePerson(It.IsAny<Person>()));
            var mockRepository = mock.Object;
            string expected = _persons[id].FirstName + " " + _persons[id].LastName;

            // Act
            var result = await PersonApi.UpdatePerson(updatedPerson, mockRepository);

            //Assert
            Assert.IsType<NoContent>(result);
            mock.Verify(m => m.UpdatePerson(It.IsAny<Person>()));
        }
    }
}