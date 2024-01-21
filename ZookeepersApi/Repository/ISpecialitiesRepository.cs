using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.ZookeepersApi.Repository
{
    public interface ISpecialitiesRepository
    {
        Task<Speciality> ChangeRelationBetweenZookeeperAndSpecialityAsync(int relationId,
            SpecialityDto specialityDto);
    }
}
