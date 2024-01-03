using Microsoft.EntityFrameworkCore;
using MicroZoo.AnimalsApi.DbContexts;
using MicroZoo.AnimalsApi.Models;
using MicroZoo.Infrastructure.Models.Animals;

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

        public async Task<List<AnimalType>> GetAllAnimalTypes() =>
            await _dbContext.AnimalTypes.ToListAsync();
        
        public async Task<List<AnimalType>> GetAnimalTypesByIds(int[] animalTypeIds)
        {
            return /*animalTypes =*/ await _dbContext.AnimalTypes
                .Where(t => animalTypeIds.Contains(t.Id)).ToListAsync(); 
        }
    }
}
