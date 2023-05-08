using MicroZoo.PersonsApi.Models;

namespace MicroZoo.PersonsApi.Repository
{
    public interface IPersonRepository
    {
        Task<Person> GetById(int id);
        Task<List<Person>> GetEmployeesOfManager(int id);
        Task<Person> CreateNewEmployee(Person employee);
        Task UpdateEmployee(Person employee);
        Task DeleteEmployee(int id);
    }
}
