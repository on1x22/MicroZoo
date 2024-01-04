using Microsoft.EntityFrameworkCore;
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

        public async Task<Animal> GetAnimal(int id)
        {
            var animal = await _dbContext.Animals.FirstOrDefaultAsync(a => a.Id == id);

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

        public async Task<List<AnimalType>> GetAllAnimalTypes() =>
            await _dbContext.AnimalTypes.ToListAsync();
        
        public async Task<List<AnimalType>> GetAnimalTypesByIds(int[] animalTypeIds)
        {
            return /*animalTypes =*/ await _dbContext.AnimalTypes
                .Where(t => animalTypeIds.Contains(t.Id)).ToListAsync(); 
        }

        // TODO: Add command SaveChanges
        public async Task AddAnimal(Animal animal)
        {
            await _dbContext.Animals.AddAsync(animal);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();

        public async Task<Animal> UpdateAnimal(int id, AnimalDto animalDto)
        {
            var animalInDb = await _dbContext.Animals.FirstOrDefaultAsync(a => a.Id == id);

            if (animalInDb == null)
                await Task.CompletedTask;

            if(animalDto == null)
                await Task.CompletedTask;

            animalInDb!.Name = animalDto!.Name;
            animalInDb!.Link = animalDto!.Link;
            animalInDb!.AnimalTypeId = animalDto!.AnimalTypeId;

            await SaveChangesAsync();

            return animalInDb;
        }
    }
}
