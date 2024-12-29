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
        Task<GetPersonResponse> GetPersonAsync(int personId);
        Task<GetPersonResponse> AddPersonAsync(PersonDto personDto);
        Task<GetPersonResponse> UpdatePersonAsync(int personId, PersonDto personDto);
        Task<GetPersonResponse> DeletePersonAsync(int personId);
        Task<GetPersonsResponse> GetSubordinatePersonnelAsync(int personId);
        Task<GetPersonsResponse> ChangeManagerForSubordinatePersonnel(int currentManagerId,
                                                                      int newManagerId);
    }
}
