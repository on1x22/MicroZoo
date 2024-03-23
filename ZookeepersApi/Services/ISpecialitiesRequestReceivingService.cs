using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.ZookeepersApi.Services
{
    public interface ISpecialitiesRequestReceivingService
    {
        Task<GetAnimalTypesResponse> GetAllSpecialities();
        Task<CheckZokeepersWithSpecialityAreExistResponse> CheckZokeepersWithSpecialityAreExist(CheckType checkType, 
            int animalTypeId);
        Task<CheckZokeepersWithSpecialityAreExistResponse> CheckZookeeperIsExist(CheckType checkType, int personId);
        Task<GetSpecialityResponse> AddSpeciality(SpecialityDto specialityDto);
        Task<GetSpecialityResponse> ChangeRelationBetweenZookeeperAndSpeciality(int relationId, SpecialityDto specialityDto);
        Task<GetAnimalTypesResponse> DeleteSpeciality(SpecialityDto specialityDto);
    }
}
