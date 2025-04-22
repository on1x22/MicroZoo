using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.Models.Persons.Dto;

namespace MicroZoo.PersonsApi.Services
{
    /// <summary>
    /// Provides processing of persons requests
    /// </summary>
    public interface IPersonsApiService
    {
        /// <summary>
        /// Asynchronous returns information about specified person from database
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<GetPersonResponse> GetPersonAsync(int personId);

        /// <summary>
        /// Asynchronous adds new person to database
        /// </summary>
        /// <param name="personDto"></param>
        /// <returns></returns>
        Task<GetPersonResponse> AddPersonAsync(PersonDto personDto);

        /// <summary>
        /// Asynchronous updates information about specified person in database
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="personDto"></param>
        /// <returns></returns>
        Task<GetPersonResponse> UpdatePersonAsync(int personId, PersonDto personDto);

        /// <summary>
        /// Asynchronous deletes person from database
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<GetPersonResponse> SoftDeletePersonAsync(int personId);


        /// <summary>
        /// Asynchronous returns information about subordinate personnel 
        /// of specified person from database
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<GetPersonsResponse> GetSubordinatePersonnelAsync(int personId);

        /// <summary>
        /// Asynchronous changes manager id for personnel in database
        /// </summary>
        /// <param name="currentManagerId"></param>
        /// <param name="newManagerId"></param>
        /// <returns></returns>
        Task<GetPersonsResponse> ChangeManagerForSubordinatePersonnel(int currentManagerId,
                                                                      int newManagerId);
    }
}
