using MassTransit;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Specialities.Dto;

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

        public async Task<GetAnimalTypesResponse> GetAllSpecialitiesAsync()
        {
            return await _receiver.GetResponseFromRabbitTask<GetAllAnimalTypesRequest,
                GetAnimalTypesResponse>(new GetAllAnimalTypesRequest(), _connectionService.AnimalsApiUrl);
        }

        public async Task<CheckZokeepersWithSpecialityAreExistResponse> CheckZokeepersWithSpecialityAreExistAsync( 
            CheckType checkType, int animalTypeId)
        {
            return await _specialitiesService.CheckZokeepersWithSpecialityAreExistAsync(checkType, animalTypeId);
        }

        public async Task<CheckZokeepersWithSpecialityAreExistResponse> CheckZookeeperIsExistAsync(CheckType checkType, int personId)
        {
            return await _specialitiesService.CheckZokeepersWithSpecialityAreExistAsync(checkType, personId);
        }

        public async Task<GetSpecialityResponse> AddSpecialityAsync(SpecialityDto specialityDto)
        {
            var personResponse = await _receiver.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                new GetPersonRequest(specialityDto.ZookeeperId), _connectionService.PersonsApiUrl);

            var animalTypeResponse = await _receiver.GetResponseFromRabbitTask<GetAnimalTypeRequest,
                GetAnimalTypeResponse>(new GetAnimalTypeRequest(specialityDto.AnimalTypeId), _connectionService.AnimalsApiUrl);

            string errorMessage = string.Empty;

            if (personResponse.Person == null)
                errorMessage += personResponse.ErrorMessage + ".\n";

            if (animalTypeResponse.AnimalType == null)
                errorMessage += animalTypeResponse.ErrorMessage;

            if (errorMessage != string.Empty)            
                return new GetSpecialityResponse() { ErrorMessage = errorMessage };
            
            return await _specialitiesService.AddSpecialityAsync(specialityDto);
        }

        public async Task<GetSpecialityResponse> ChangeRelationBetweenZookeeperAndSpecialityAsync(int relationId, 
            SpecialityDto specialityDto)
        {
            var person = await _receiver.GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                new GetPersonRequest(specialityDto.ZookeeperId), _connectionService.PersonsApiUrl);

            var animalType = await _receiver.GetResponseFromRabbitTask<GetAnimalTypeRequest,
                GetAnimalTypeResponse>(new GetAnimalTypeRequest(specialityDto.AnimalTypeId), 
                _connectionService.AnimalsApiUrl);

            string errorMessage = string.Empty;

            if (person.Person == null)
                errorMessage += person.ErrorMessage + ".\n";

            if (animalType.AnimalType == null)
                errorMessage += animalType.ErrorMessage;

            if (errorMessage != string.Empty)
                return new GetSpecialityResponse() { ErrorMessage = errorMessage };

            return await _specialitiesService.ChangeRelationBetweenZookeeperAndSpecialityAsync(relationId, 
                specialityDto);
        }

        public async Task<GetAnimalTypesResponse> DeleteSpecialityAsync(SpecialityDto specialityDto)
        {
            var specialitiesResponse = await _specialitiesService.DeleteSpecialityAsync(specialityDto);

            if (specialitiesResponse.Specialities == null)
                return new GetAnimalTypesResponse() { ErrorMessage = specialitiesResponse.ErrorMessage };

            var animalTypesIds = specialitiesResponse.Specialities.Select(x => x.AnimalTypeId).ToArray();

            return await _receiver.GetResponseFromRabbitTask<GetAnimalTypesByIdsRequest,
                GetAnimalTypesResponse>(new GetAnimalTypesByIdsRequest(animalTypesIds), _connectionService.AnimalsApiUrl);
        }
    }
}
