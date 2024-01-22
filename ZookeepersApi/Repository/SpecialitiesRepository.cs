using Microsoft.EntityFrameworkCore;
using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.Infrastructure.Models.Specialities.Dto;
using MicroZoo.ZookeepersApi.DBContext;

namespace MicroZoo.ZookeepersApi.Repository
{
    public class SpecialitiesRepository : ISpecialitiesRepository
    {
        private readonly ZookeeperDBContext _dBContext;

        public SpecialitiesRepository(ZookeeperDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<List<Speciality>> GetSpecialitiesByZookeeperIdAsync(int zookeeperId) =>
            await _dBContext.Specialities.Where(s => s.ZookeeperId == zookeeperId).ToListAsync();
        

        public async Task<Speciality> ChangeRelationBetweenZookeeperAndSpecialityAsync(int relationId, SpecialityDto specialityDto)
        {
            var speciality = await _dBContext.Specialities.FirstOrDefaultAsync(s => s.Id == relationId);

            if (speciality == null)
                return default;

            speciality.ZookeeperId = specialityDto.ZookeeperId;
            speciality.AnimalTypeId = specialityDto.AnimalTypeId;
            await SaveChangesAsync();

            return speciality;
        }

        /// <summary>
        /// Returns true, if one o more zokeepers with speciality exist in database
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>True of false</returns>
        public async Task<bool> CheckZokeepersWithSpecialityAreExistAsync(int animalTypeId) =>
            await _dBContext.Specialities.AnyAsync(s => s.AnimalTypeId == animalTypeId);

        /// <summary>
        /// Returns true, if a zookeeper is exists in database
        /// </summary>
        /// <param name="zookeeperId"></param>
        /// <returns>True of false</returns>
        public async Task<bool> CheckZookeeperIsExistAsync(int zookeeperId) =>
            await _dBContext.Specialities.AnyAsync(s => s.ZookeeperId == zookeeperId);

        public async Task DeleteSpecialityAsync(SpecialityDto specialityDto)
        {
            await _dBContext.Specialities.Where(s => s.ZookeeperId == specialityDto.ZookeeperId &&
                s.AnimalTypeId == specialityDto.AnimalTypeId)
                .ExecuteDeleteAsync();

            _dBContext.SaveChanges();
        }

        private async Task SaveChangesAsync() =>
            await _dBContext.SaveChangesAsync();
    }
}
