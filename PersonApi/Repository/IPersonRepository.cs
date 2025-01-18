using MicroZoo.Infrastructure.Models.Persons;

namespace MicroZoo.PersonsApi.Repository
{
    public interface IPersonRepository
    {
        Task<Person> GetPersonAsync(int personId);        
        Task<Person> AddPersonAsync(Person person);
        Task<Person> UpdatePersonAsync(int personId, Person personDto);
        Task<Person> SoftDeletePersonAsync(/*int personId*/Person person);
        Task<bool> CheckPersonIsManager(int personId);

        Task<List<Person>> GetSubordinatePersonnelAsync(int personId);
        Task<List<Person>> ChangeManagerForSubordinatePersonnel(int currentManagerId,
                                                                int newManagerId);
    }
}
