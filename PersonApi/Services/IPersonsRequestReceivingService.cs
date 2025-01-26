using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.Models.Persons.Dto;

namespace MicroZoo.PersonsApi.Services
{
    /// <summary>
    /// Interface for processing request from controllers and RabbitMq consumers. If request 
    /// require interconnection with other microservices this actions doing here
    /// </summary>
    public interface IPersonsRequestReceivingService
    {
        /// <summary>
        /// Asynchronous returns information about specified person
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<GetPersonResponse> GetPersonAsync(int personId);

        /// <summary>
        /// Asynchronous adds new person
        /// </summary>
        /// <param name="personDto"></param>
        /// <returns></returns>
        Task<GetPersonResponse> AddPersonAsync(PersonDto personDto);

        /// <summary>
        /// Asynchronous updates information about specified person
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="personDto"></param>
        /// <returns></returns>
        Task<GetPersonResponse> UpdatePersonAsync(int personId, PersonDto personDto);

        /// <summary>
        /// Asynchronous deletes person
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<GetPersonResponse> SoftDeletePersonAsync(int personId, string accessToken);

        /// <summary>
        /// Asynchronous returns information about subordinate personnel 
        /// of specified person
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<GetPersonsResponse> GetSubordinatePersonnelAsync(int personId);

        /// <summary>
        /// Asynchronous changes manager id for personnel
        /// </summary>
        /// <param name="currentManagerId"></param>
        /// <param name="newManagerId"></param>
        /// <returns></returns>
        Task<GetPersonsResponse> ChangeManagerForSubordinatePersonnel(int currentManagerId,
                                                                      int newManagerId);
    }
}
