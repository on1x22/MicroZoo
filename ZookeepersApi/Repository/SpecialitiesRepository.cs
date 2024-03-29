﻿using Microsoft.EntityFrameworkCore;
using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.Infrastructure.Models.Specialities.Dto;
using MicroZoo.ZookeepersApi.DBContext;
using System.Runtime.CompilerServices;

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

        public async Task<Speciality> AddSpecialityAsync(SpecialityDto specialityDto)
        {
            var speciality = specialityDto.ToSpeciality();

            await _dBContext.Specialities.AddAsync(speciality);
            await SaveChangesAsync();

            return speciality;
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

        public async Task<Speciality> DeleteSpecialityAsync(SpecialityDto specialityDto)
        {
            var speciality = await _dBContext.Specialities.FirstOrDefaultAsync(s => 
            s.ZookeeperId == specialityDto.ZookeeperId && s.AnimalTypeId == specialityDto.AnimalTypeId);

            _dBContext.Specialities.Remove(speciality);

            await SaveChangesAsync();

            return speciality;
        }


        private async Task SaveChangesAsync() =>
            await _dBContext.SaveChangesAsync();
    }
}
