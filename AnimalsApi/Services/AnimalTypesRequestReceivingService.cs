using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Services
{
    public class AnimalTypesRequestReceivingService : IAnimalTypesRequestReceivingService
    {
        private readonly IAnimalsApiService _animalsService;
        private readonly IConnectionService _connectionService;
        private readonly IResponsesReceiverFromRabbitMq _receiverFromRabbitMq;

        public AnimalTypesRequestReceivingService(IAnimalsApiService animalsService,
            IServiceProvider provider,
            IConnectionService connectionService,
            IResponsesReceiverFromRabbitMq receiverFromRabbitMq)
        {
            _animalsService = animalsService;
            _connectionService = connectionService;
            _receiverFromRabbitMq = receiverFromRabbitMq;
        }

        public async Task<GetAnimalTypesResponse> GetAllAnimalTypesAsync() =>
            await _animalsService.GetAllAnimalTypesAsync();       

        public async Task<GetAnimalTypeResponse> GetAnimalTypeAsync(int animalTypeId) =>
            await _animalsService.GetAnimalTypeAsync(animalTypeId);

        public async Task<GetAnimalTypeResponse> AddAnimalTypeAsync(AnimalTypeDto animalTypeDto) =>
            await _animalsService.AddAnimalTypeAsync(animalTypeDto);
        
        public async Task<GetAnimalTypeResponse> UpdateAnimalTypeAsync(int animalTypeId, 
            AnimalTypeDto animalTypeDto) =>
            await _animalsService.UpdateAnimalTypeAsync(animalTypeId, animalTypeDto);

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

        public async Task<GetAnimalTypesResponse> GetAnimalTypesByIdsAsync(int[] animalTypesIds) =>
            await _animalsService.GetAnimalTypesByIdsAsync(animalTypesIds);
    }
}
