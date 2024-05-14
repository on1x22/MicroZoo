using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface ISpecialitiesRequestReceivingService
    {
        Task<GetAnimalTypesResponse> GetAllSpecialitiesAsync();
        Task<CheckZokeepersWithSpecialityAreExistResponse> CheckZokeepersWithSpecialityAreExistAsync(CheckType checkType, 
            int animalTypeId);
        Task<CheckZokeepersWithSpecialityAreExistResponse> CheckZookeeperIsExistAsync(CheckType checkType, int personId);
        Task<GetSpecialityResponse> AddSpecialityAsync(SpecialityDto specialityDto);
        Task<GetSpecialityResponse> ChangeRelationBetweenZookeeperAndSpecialityAsync(int relationId, SpecialityDto specialityDto);
        Task<GetAnimalTypesResponse> DeleteSpecialityAsync(SpecialityDto specialityDto);
    }
}
