﻿using Microsoft.EntityFrameworkCore;
using MicroZoo.AnimalsApi.DbContexts;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Repository
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly AnimalDbContext _dbContext;

        public AnimalRepository(AnimalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Animal>> GetAllAnimalsAsync()
        {
            return await _dbContext.Animals.Where(a => a.Deleted == false).ToListAsync();
        }

        public async Task<Animal> GetAnimalAsync(int animalId)
        {
            var animal = await _dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId
                                                                      && a.Deleted == false);

            if (animal == null)
                return default;

            return animal;
        }

        public async Task<List<Animal>> GetAnimalsByTypes(List<int> animalTypeIds)
        {
            var typesFromDb = _dbContext.AnimalTypes.Select(at => at.Id);
            var allTypesExistInDb = typesFromDb.All(x => animalTypeIds.Contains(x));
            
            if (allTypesExistInDb) return null;

            return await _dbContext.Animals.Where(a => animalTypeIds.Contains(a.AnimalTypeId)).ToListAsync();                                                                    ;
        }

        public async Task<List<AnimalType>> GetAllAnimalTypesAsync() =>
            await _dbContext.AnimalTypes.Where(at => at.Deleted == false).ToListAsync();
        
        public async Task<Animal> AddAnimalAsync(Animal animal)
        {
            //if (!await IsAnimalTypeExist(animalDto!.AnimalTypeId))
            //    return default;

            //var animal = AnimalDto.DtoToAnimal(animalDto);

            await _dbContext.Animals.AddAsync(animal);
            await SaveChangesAsync();
            return animal;
        }

        public async Task<Animal> UpdateAnimalAsync(int animalId, AnimalDto animalDto)
        {
            if (animalDto == null)
                //await Task.CompletedTask;
                return default;

            var animalInDb = await _dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId
                                                                          && a.Deleted == false);

            if (animalInDb == null)
                //await Task.CompletedTask;
                return default;
                                    
            //if (!await IsAnimalTypeExist(animalDto!.AnimalTypeId))
            //    return default;

            animalInDb!.Name = animalDto!.Name;
            animalInDb!.Link = animalDto!.Link;
            animalInDb!.AnimalTypeId = animalDto!.AnimalTypeId;

            await SaveChangesAsync();

            return animalInDb;
        }

        public async Task<Animal> DeleteAnimalAsync(int animalId)
        {
            var animal = await _dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId);
            if (animal == null)
                return default;

            //_dbContext.Animals.Remove(animal);
            animal.Deleted = true;
            await SaveChangesAsync();

            return animal;
        }

        public async Task<List<Animal>> GetAnimalsByTypesAsync(int[] animalTypeIds)
        {
            var typesFromDb = _dbContext.AnimalTypes.Where(at => at.Deleted == false)
                                                    .Select(at => at.Id);
            var allTypesExistInDb = typesFromDb.All(x => animalTypeIds.Contains(x));

            if (allTypesExistInDb) return null;

            return await _dbContext.Animals.Where(a => animalTypeIds.Contains(a.AnimalTypeId)
                                                  && a.Deleted == false)
                                           .ToListAsync(); ;
        }

        public async Task<bool> IsAnimalTypeExist(int animalTypeId) =>
            await _dbContext.AnimalTypes.AnyAsync(t => t.Id == animalTypeId && t.Deleted == false);

        public async Task<AnimalType> GetAnimalTypeAsync(int animalTypeId)
        {
            var animalType = await _dbContext.AnimalTypes.FirstOrDefaultAsync(t => 
                t.Id == animalTypeId && t.Deleted == false);
            if (animalType == null)
                return default;

            return animalType;
        }

        public async Task<AnimalType> AddAnimalTypeAsync(AnimalType animalType)
        {
            //var animalType = AnimalTypeDto.DtoToAnimalType(animalType);

            await _dbContext.AnimalTypes.AddAsync(animalType);
            await SaveChangesAsync();
            return animalType;
        }

        public async Task<AnimalType> UpdateAnimalTypeAsync(int animaltypeId, AnimalTypeDto animalTypeDto)
        {
            if (animalTypeDto == null)
                //await Task.CompletedTask;
                return default!;
            
            var animalTypeInDb = await _dbContext.AnimalTypes.FirstOrDefaultAsync(a => 
            a.Id == animaltypeId && a.Deleted == false);

            if (animalTypeInDb == null)
                //await Task.CompletedTask;            
                return default!;

            animalTypeInDb!.Description = animalTypeDto!.Description;

            await SaveChangesAsync();

            return animalTypeInDb;
        }

        public async Task<AnimalType> DeleteAnimalTypeAsync(int animalTypeId)
        {
            var animalType = await _dbContext.AnimalTypes.FirstOrDefaultAsync(a => 
                                                                              a.Id == animalTypeId);
            if (animalType == null)
                return default;

            //_dbContext.AnimalTypes.Remove(animalType);
            animalType.Deleted = true;
            await SaveChangesAsync();

            return animalType;
        }

        public async Task<List<AnimalType>> GetAnimalTypesByIdsAsync(int[] animalTypesIds)
        {
            return await _dbContext.AnimalTypes
                .Where(t => animalTypesIds.Contains(t.Id) && t.Deleted == false).ToListAsync();
        }

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}
