using Microsoft.EntityFrameworkCore;
using microZoo.Infrastructure.Exceptions;
using MicroZoo.AnimalsApi.DbContexts;
using MicroZoo.AnimalsApi.Models;
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
            return await _dbContext.Animals.ToListAsync();
        }

        public async Task<Animal> GetAnimalAsync(int animalId)
        {
            var animal = await _dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId);

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

        public async Task<List<Animal>> GetAnimalsByTypes2(int[] animalTypeIds)
        {
            var typesFromDb = _dbContext.AnimalTypes.Select(at => at.Id);
            var allTypesExistInDb = typesFromDb.All(x => animalTypeIds.Contains(x));

            if (allTypesExistInDb) return null;

            return await _dbContext.Animals.Where(a => animalTypeIds.Contains(a.AnimalTypeId)).ToListAsync(); ;
        }

        public async Task<List<AnimalType>> GetAllAnimalTypesAsync() =>
            await _dbContext.AnimalTypes.ToListAsync();
        
        public async Task<List<AnimalType>> GetAnimalTypesByIds(int[] animalTypeIds)
        {
            return /*animalTypes =*/ await _dbContext.AnimalTypes
                .Where(t => animalTypeIds.Contains(t.Id)).ToListAsync(); 
        }

        public async Task<Animal> AddAnimalAsync(AnimalDto animalDto)
        {
            //if (!await IsAnimalTypeExist(animalDto!.AnimalTypeId))
            //    return default;

            var animal = AnimalDto.DtoToAnimal(animalDto);

            await _dbContext.Animals.AddAsync(animal);
            await SaveChangesAsync();
            return animal;
        }

        public async Task<Animal> UpdateAnimalAsync(int animalId, AnimalDto animalDto)
        {
            if (animalDto == null)
                await Task.CompletedTask;

            var animalInDb = await _dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId);

            if (animalInDb == null)
                await Task.CompletedTask;
                                    
            if (!await IsAnimalTypeExist(animalDto!.AnimalTypeId))
                return default;

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

            _dbContext.Animals.Remove(animal);
            await SaveChangesAsync();

            return animal;
        }
        
        public async Task<bool> IsAnimalTypeExist(int animalTypeId) =>
            await _dbContext.AnimalTypes.AnyAsync(t => t.Id == animalTypeId);

        public async Task<AnimalType> GetAnimalTypeAsync(int animalTypeId)
        {
            var animalType = await _dbContext.AnimalTypes.FirstOrDefaultAsync(t => t.Id == animalTypeId);
            if (animalType == null)
                return default;

            return animalType;
        }

        public async Task<AnimalType> AddAnimalTypeAsync(AnimalTypeDto animalTypeDto)
        {
            var animalType = AnimalTypeDto.DtoToAnimalType(animalTypeDto);

            await _dbContext.AnimalTypes.AddAsync(animalType);
            await SaveChangesAsync();
            return animalType;
        }

        public async Task<AnimalType> UpdateAnimalTypeAsync(int animaltypeId, AnimalTypeDto animalTypeDto)
        {
            if (animalTypeDto == null)
                await Task.CompletedTask;
            
            var animalTypeInDb = await _dbContext.AnimalTypes.FirstOrDefaultAsync(a => a.Id == animaltypeId);

            if (animalTypeInDb == null)
                await Task.CompletedTask;            

            animalTypeInDb!.Description = animalTypeDto!.Description;

            await SaveChangesAsync();

            return animalTypeInDb;
        }

        public async Task<AnimalType> DeleteAnimalTypeAsync(int animalTypeId)
        {
            var animalType = await _dbContext.AnimalTypes.FirstOrDefaultAsync(a => a.Id == animalTypeId);
            if (animalType == null)
                return default;

            _dbContext.AnimalTypes.Remove(animalType);
            await SaveChangesAsync();

            return animalType;
        }

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}
