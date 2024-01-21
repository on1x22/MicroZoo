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

        private async Task SaveChangesAsync() =>
            await _dBContext.SaveChangesAsync();
    }
}
