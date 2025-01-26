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

        /// <summary>
        /// Asynchronous returns information about specified person
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<GetPersonResponse> GetPersonAsync(int personId) =>
            await _personsService.GetPersonAsync(personId);

        /// <summary>
        /// Asynchronous adds new person
        /// </summary>
        /// <param name="personDto"></param>
        /// <returns></returns>
        public async Task<GetPersonResponse> AddPersonAsync(PersonDto personDto) =>
            await _personsService.AddPersonAsync(personDto);

        /// <summary>
        /// Asynchronous updates information about specified person
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="personDto"></param>
        /// <returns></returns>
        public async Task<GetPersonResponse> UpdatePersonAsync(int personId, PersonDto personDto) =>
            await _personsService.UpdatePersonAsync(personId, personDto);

        /// <summary>
        /// Asynchronous deletes person
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<GetPersonResponse> SoftDeletePersonAsync(int personId, string accessToken)
        {
            var response = new GetPersonResponse();

            // This action is in question. This check should be performed
            // in the upstream microservice
            var isZookeeperExists = await _receiverFromRabbitMq.
                GetResponseFromRabbitTask<CheckZokeepersWithSpecialityAreExistRequest,
                CheckZokeepersWithSpecialityAreExistResponse>
                (new CheckZokeepersWithSpecialityAreExistRequest(CheckType.Person, personId,
                accessToken), _connectionService.ZookeepersApiUrl);

            /*if (isZookeeperExists.ActionResult != null)
            {
                response.ActionResult = isZookeeperExists.ActionResult;
                return response;
            }*/

            if (isZookeeperExists./*ResponseError*/ErrorCode != null)
            {
                //response.ResponseError = isZookeeperExists.ResponseError;
                response.ErrorCode = isZookeeperExists.ErrorCode;
                response.ErrorMessage = isZookeeperExists.ErrorMessage;
                return response;
            }

            if (isZookeeperExists.IsThereZookeeperWithThisSpeciality)
            {
                response.ErrorMessage = $"There is zookeeper with id={personId}. " +
                    "Before deleting a zookeeper, you must remove zookeeper " +
                    "associations with all specialties.";
                
                return response;
            }

            response = await _personsService.SoftDeletePersonAsync(personId);

            return response;
        }

        /// <summary>
        /// Asynchronous returns information about subordinate personnel 
        /// of specified person
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<GetPersonsResponse> GetSubordinatePersonnelAsync(int personId) =>
            await _personsService.GetSubordinatePersonnelAsync(personId);

        /// <summary>
        /// Asynchronous changes manager id for personnel
        /// </summary>
        /// <param name="currentManagerId"></param>
        /// <param name="newManagerId"></param>
        /// <returns></returns>
        public async Task<GetPersonsResponse> ChangeManagerForSubordinatePersonnel(int currentManagerId, 
                                                                                   int newManagerId) =>
            await _personsService.ChangeManagerForSubordinatePersonnel(currentManagerId, 
                newManagerId);
    }
}
