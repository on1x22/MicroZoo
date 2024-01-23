using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.ZookeepersApi.Repository
{
    public interface ISpecialitiesRepository
    {
        Task<List<Speciality>> GetSpecialitiesByZookeeperIdAsync(int zookeeperId);

        /// <summary>
        /// Returns true, if one o more zokeepers with speciality exist in database
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>True of false</returns>
        Task<bool> CheckZokeepersWithSpecialityAreExistAsync(int animalTypeId);

        /// <summary>
        /// Returns true, if a zookeeper is exists in database
        /// </summary>
        /// <param name="zookeeperId"></param>
        /// <returns>True of false</returns>
        Task<bool> CheckZookeeperIsExistAsync(int zookeeperId);

        Task<Speciality> AddSpecialityAsync(SpecialityDto specialityDto);

        Task<Speciality> ChangeRelationBetweenZookeeperAndSpecialityAsync(int relationId,
            SpecialityDto specialityDto);

        Task DeleteSpecialityAsync(SpecialityDto specialityDto);
    }
}
