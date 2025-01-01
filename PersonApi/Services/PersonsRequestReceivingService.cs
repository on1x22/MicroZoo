using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
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
        private readonly IResponsesReceiverFromRabbitMq _receiverFromRabbitMq;
        private readonly IConnectionService _connectionService;

        /// <summary>
        /// Initializes a new instance of <see cref="PersonsRequestReceivingService"/> class 
        /// </summary>
        /// <param name="personsService"></param>
        /// <param name="receiverFromRabbitMq"></param>
        /// <param name="connectionService"></param>
        public PersonsRequestReceivingService(IPersonsApiService personsService,
            IResponsesReceiverFromRabbitMq receiverFromRabbitMq,
            IConnectionService connectionService)
        {
            _personsService = personsService;
            _receiverFromRabbitMq = receiverFromRabbitMq;
            _connectionService = connectionService;
        }

        public async Task<GetPersonResponse> GetPersonAsync(int personId) =>
            await _personsService.GetPersonAsync(personId);

        public async Task<GetPersonResponse> AddPersonAsync(PersonDto personDto) =>
            await _personsService.AddPersonAsync(personDto);

        public async Task<GetPersonResponse> UpdatePersonAsync(int personId, PersonDto personDto) =>
            await _personsService.UpdatePersonAsync(personId, personDto);

        public async Task<GetPersonResponse> DeletePersonAsync(int personId)
        {
            var response = new GetPersonResponse();

            var isZookeeperExists = await _receiverFromRabbitMq.
                GetResponseFromRabbitTask<CheckZokeepersWithSpecialityAreExistRequest,
                CheckZokeepersWithSpecialityAreExistResponse>
                (new CheckZokeepersWithSpecialityAreExistRequest(CheckType.Person, personId),
                _connectionService.ZookeepersApiUrl);

            if (isZookeeperExists.IsThereZookeeperWithThisSpeciality)
            {
                response.ErrorMessage = $"There is zookeeper with id={personId}. " +
                    "Before deleting a zookeeper, you must remove zookeeper " +
                    "associations with all specialties.";
                
                return response;
            }

            response = await _personsService.DeletePersonAsync(personId);

            return response;
        }

        public async Task<GetPersonsResponse> GetSubordinatePersonnelAsync(int personId) =>
            await _personsService.GetSubordinatePersonnelAsync(personId);

        public async Task<GetPersonsResponse> ChangeManagerForSubordinatePersonnel(int currentManagerId, 
                                                                                   int newManagerId) =>
            await _personsService.ChangeManagerForSubordinatePersonnel(currentManagerId, 
                newManagerId);
    }
}
