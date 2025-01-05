using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface ISpecialitiesRequestReceivingService
    {
        Task<GetAnimalTypesResponse> GetAllSpecialitiesAsync(string accessToken);
        Task<CheckZokeepersWithSpecialityAreExistResponse> 
            CheckZokeepersWithSpecialityAreExistAsync(CheckType checkType, int animalTypeId);
        Task<CheckZokeepersWithSpecialityAreExistResponse> CheckZookeeperIsExistAsync(
            CheckType checkType, int personId);
        Task<GetSpecialityResponse> AddSpecialityAsync(SpecialityDto specialityDto, string accessToken);
        Task<GetSpecialityResponse> ChangeRelationBetweenZookeeperAndSpecialityAsync(int relationId, SpecialityDto specialityDto, string accessToken);
        Task<GetAnimalTypesResponse> DeleteSpecialityAsync(SpecialityDto specialityDto,
            string accessToken);
    }
}
