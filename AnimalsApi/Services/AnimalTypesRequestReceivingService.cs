using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Services
{
    /// <summary>
    /// Provides sending requests to another microservices if it's necessary.
    /// If sending not required then processing moves on to the next service
    /// </summary>
    public class AnimalTypesRequestReceivingService : IAnimalTypesRequestReceivingService
    {
        private readonly IAnimalsApiService _animalsService;
        private readonly IConnectionService _connectionService;
        private readonly IResponsesReceiverFromRabbitMq _receiverFromRabbitMq;

        /// <summary>
        /// Initialize a new instance of <see cref="AnimalTypesRequestReceivingService"/> class
        /// </summary> 
        public AnimalTypesRequestReceivingService(IAnimalsApiService animalsService,
            IServiceProvider provider,
            IConnectionService connectionService,
            IResponsesReceiverFromRabbitMq receiverFromRabbitMq)
        {
            _animalsService = animalsService;
            _connectionService = connectionService;
            _receiverFromRabbitMq = receiverFromRabbitMq;
        }

        /// <summary>
        /// Asynchronous returns information about all animal types in database
        /// </summary>
        /// <returns>GetAnimalTypesResponse</returns>
        public async Task<GetAnimalTypesResponse> GetAllAnimalTypesAsync() =>
            await _animalsService.GetAllAnimalTypesAsync();

        /// <summary>
        /// Asynchronous returns information about specified animal type from database
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>GetAnimalTypeResponse</returns>
        public async Task<GetAnimalTypeResponse> GetAnimalTypeAsync(int animalTypeId) =>
            await _animalsService.GetAnimalTypeAsync(animalTypeId);

        /// <summary>
        /// Asynchronous adds new animal type to database
        /// </summary>
        /// <param name="animalTypeDto"></param>
        /// <returns>GetAnimalTypeResponse with added animal type</returns>
        public async Task<GetAnimalTypeResponse> AddAnimalTypeAsync(AnimalTypeDto animalTypeDto) =>
            await _animalsService.AddAnimalTypeAsync(animalTypeDto);

        /// <summary>
        /// Asynchronous updates information about specified animal type in database
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <param name="animalTypeDto"></param>
        /// <returns>GetAnimalTypeResponse with updated animal type</returns>
        public async Task<GetAnimalTypeResponse> UpdateAnimalTypeAsync(int animalTypeId, 
            AnimalTypeDto animalTypeDto) =>
            await _animalsService.UpdateAnimalTypeAsync(animalTypeId, animalTypeDto);

        /// <summary>
        /// Asynchronous deletes animal type from database
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <param name="accessToken"></param>
        /// <returns>GetAnimalTypeResponse with deleted animal type</returns>
        public async Task<GetAnimalTypeResponse> DeleteAnimalTypeAsync(int animalTypeId,
                                                                       string accessToken)
        {
            var response = new GetAnimalTypeResponse();

            // This action is in question. This check should be performed
            // in the upstream microservice
            var isThereZokeeperWithSpecialty = await _receiverFromRabbitMq.
                GetResponseFromRabbitTask<CheckZokeepersWithSpecialityAreExistRequest,
                CheckZokeepersWithSpecialityAreExistResponse>
                (new CheckZokeepersWithSpecialityAreExistRequest(CheckType.AnimalType, 
                animalTypeId, accessToken), _connectionService.ZookeepersApiUrl);

            if (isThereZokeeperWithSpecialty.IsThereZookeeperWithThisSpeciality)
            {
                response.ErrorMessage = $"There are zookeepers with specialization {animalTypeId}. " +
                    "Before deleting a specialty, you must remove the zookeepers " +
                    "association with that specialty.";
                response.ErrorCode = ErrorCodes.BadRequest400;
                return response;
            }

            return await _animalsService.DeleteAnimalTypeAsync(animalTypeId);
        }

        /// <summary>
        /// Asynchronous returns information about animal types which id matchs with specified
        /// </summary>
        /// <param name="animalTypesIds"></param>
        /// <returns>GetAnimalTypesResponse</returns>
        public async Task<GetAnimalTypesResponse> GetAnimalTypesByIdsAsync(int[] animalTypesIds) =>
            await _animalsService.GetAnimalTypesByIdsAsync(animalTypesIds);
    }
}
