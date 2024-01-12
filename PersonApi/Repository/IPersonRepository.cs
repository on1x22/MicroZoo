using MicroZoo.PersonsApi.Models;
using MicroZoo.Infrastructure.Models.Persons;

namespace MicroZoo.PersonsApi.Repository
{
    public interface IPersonRepository
    {
        Task<Person> GetPersonAsync(int personId);
        Task<List<Person>> GetEmployeesOfManager(int id);
        Task<Person> CreateNewPerson(Person employee);
        Task UpdatePerson(Person employee);
        Task DeletePerson(int id);
    }
}
