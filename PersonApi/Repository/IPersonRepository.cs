using MicroZoo.Infrastructure.Models.Persons;

namespace MicroZoo.PersonsApi.Repository
{
    /// <summary>
    /// Provides processing of persons requests to database
    /// </summary>
    public interface IPersonRepository
    {
        /// <summary>
        /// Asynchronous returns information about specified person from database
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<Person> GetPersonAsync(int personId);

        /// <summary>
        /// Asynchronous adds new person to database
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task<Person> AddPersonAsync(Person person);

        /// <summary>
        /// Asynchronous updates information about specified person in database
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="personDto"></param>
        /// <returns></returns>
        Task<Person> UpdatePersonAsync(int personId, Person personDto);

        /// <summary>
        /// Asynchronous deletes person from database
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task<Person> SoftDeletePersonAsync(/*int personId*/Person person);

        /// <summary>
        /// Asynchronous checks whether specified person in database or not
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<bool> CheckPersonIsManager(int personId);

        /// <summary>
        /// Asynchronous returns information about subordinate personnel 
        /// of specified person from database
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<List<Person>> GetSubordinatePersonnelAsync(int personId);

        /// <summary>
        /// Asynchronous changes manager id for personnel in database
        /// </summary>
        /// <param name="currentManagerId"></param>
        /// <param name="newManagerId"></param>
        /// <returns></returns>
        Task<List<Person>> ChangeManagerForSubordinatePersonnel(int currentManagerId,
                                                                int newManagerId);
    }
}
