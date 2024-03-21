using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;

namespace MicroZoo.ZookeepersApi.Services
{
    public class SpecialitiesRequestReceivingService : ISpecialitiesRequestReceivingService
    {
        private readonly ISpecialitiesService _specialitiesService;
        private readonly IResponsesReceiverFromRabbitMq _receiver;
        private readonly IConnectionService _connectionService;

        public SpecialitiesRequestReceivingService(ISpecialitiesService specialitiesService, 
            IResponsesReceiverFromRabbitMq receiver, IConnectionService connectionService)
        {
            _specialitiesService = specialitiesService;
            _receiver = receiver;
            _connectionService = connectionService;
        }

        public async Task<GetAnimalTypesResponse> GetAllSpecialities()
        {
            return await _receiver.GetResponseFromRabbitTask<GetAllAnimalTypesRequest,
                GetAnimalTypesResponse>(new GetAllAnimalTypesRequest(), _connectionService.AnimalsApiUrl);
        }

        public async Task<CheckZokeepersWithSpecialityAreExistResponse> CheckZokeepersWithSpecialityAreExist( 
            CheckType checkType, int animalTypeId)
        {
            return await _specialitiesService.CheckZokeepersWithSpecialityAreExistAsync(checkType, animalTypeId);
        }

        public async Task<CheckZokeepersWithSpecialityAreExistResponse> CheckZookeeperIsExist(CheckType checkType, int personId)
        {
            return await _specialitiesService.CheckZokeepersWithSpecialityAreExistAsync(checkType, personId);
        }
    }
}
