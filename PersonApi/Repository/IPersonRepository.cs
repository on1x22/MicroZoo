using MicroZoo.Infrastructure.Models.Persons;
using MicroZoo.Infrastructure.Models.Persons.Dto;

namespace MicroZoo.PersonsApi.Repository
{
    public interface IPersonRepository
    {
        Task<Person> GetPersonAsync(int personId);        
        Task<Person> AddPersonAsync(PersonDto person);
        Task<Person> UpdatePersonAsync(int personId, PersonDto personDto);
        Task<Person> DeletePersonAsync(int personId);
        Task<bool> CheckPersonIsManager(int personId);

        Task<List<Person>> GetSubordinatePersonnelAsync(int personId);
        Task<List<Person>> ChangeManagerForSubordinatePersonnel(int currentManagerId,
                                                                int newManagerId);
    }
}
