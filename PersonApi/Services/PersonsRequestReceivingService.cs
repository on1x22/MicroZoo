using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.Models.Persons.Dto;

namespace MicroZoo.PersonsApi.Services
{
    /// <summary>
    /// Procceses request from controllers and RabbitMq consumers. Interconnections with other 
    /// microservices doing here 
    /// </summary>
    public class PersonsRequestReceivingService : IPersonsRequestReceivingService
    {
        private readonly IPersonsApiService _personsService;

        /// <summary>
        /// Initializes a new instance of <see cref="PersonsRequestReceivingService"/> class 
        /// </summary>
        /// <param name="personsService"></param>
        public PersonsRequestReceivingService(IPersonsApiService personsService)
        {
            _personsService = personsService;
        }

        public async Task<GetPersonResponse> GetPersonAsync(int personId) =>
            await _personsService.GetPersonAsync(personId);

        public async Task<GetPersonResponse> AddPersonAsync(PersonDto personDto) =>
            await _personsService.AddPersonAsync(personDto);

        public async Task<GetPersonResponse> UpdatePersonAsync(int personId, PersonDto personDto) =>
            await _personsService.UpdatePersonAsync(personId, personDto);

        public async Task<GetPersonResponse> DeletePersonAsync(int personId) =>
            await _personsService.DeletePersonAsync(personId);

        public async Task<GetPersonsResponse> GetSubordinatePersonnelAsync(int personId) =>
            await _personsService.GetSubordinatePersonnelAsync(personId);

        public async Task<GetPersonsResponse> ChangeManagerForSubordinatePersonnel(int currentManagerId, 
                                                                                   int newManagerId) =>
            await _personsService.ChangeManagerForSubordinatePersonnel(currentManagerId, 
                newManagerId);
    }
}
