using Microsoft.EntityFrameworkCore;
using MicroZoo.AnimalsApi.DbContexts;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Repository
{
    /// <summary>
    /// Provides processing of animals and animal types requests to database
    /// </summary>
    public class AnimalRepository : IAnimalRepository
    {
        private readonly AnimalDbContext _dbContext;

        /// <summary>
        /// Initialize a new instance of <see cref="AnimalRepository"/> class
        /// </summary> 
        public AnimalRepository(AnimalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Asynchronous returns information about all animals from database
        /// </summary>
        /// <returns>List of animals</returns>
        public async Task<List<Animal>> GetAllAnimalsAsync()
        {
            return await _dbContext.Animals.Where(a => a.Deleted == false).ToListAsync();
        }

        /// <summary>
        /// Asynchronous returns information about specified animal from database
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns>Animal data</returns>
        public async Task<Animal> GetAnimalAsync(int animalId)
        {
            var animal = await _dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId
                                                                      && a.Deleted == false);

            if (animal == null)
                return default!;

            return animal;
        }

        /// <summary>
        /// Asynchronous returns information about animals which types matchs with specified
        /// </summary>
        /// <param name="animalTypeIds"></param>
        /// <returns>List of animals</returns>
        public async Task<List<Animal>> GetAnimalsByTypesAsync(List<int> animalTypeIds)
        {
            var typesFromDb = _dbContext.AnimalTypes.Select(at => at.Id);
            var allTypesExistInDb = typesFromDb.All(x => animalTypeIds.Contains(x));
            
            if (allTypesExistInDb) return default!;

            return await _dbContext.Animals.Where(a => animalTypeIds.Contains(a.AnimalTypeId)).ToListAsync();                                                                    ;
        }

        /// <summary>
        /// Asynchronous returns information about all animal types in database
        /// </summary>
        /// <returns>List of animal types</returns>
        public async Task<List<AnimalType>> GetAllAnimalTypesAsync() =>
            await _dbContext.AnimalTypes.Where(at => at.Deleted == false).ToListAsync();

        /// <summary>
        /// Asynchronous adds new animal to database
        /// </summary>
        /// <param name="animal"></param>
        /// <returns>Added animal data</returns>
        public async Task<Animal> AddAnimalAsync(Animal animal)
        {
            await _dbContext.Animals.AddAsync(animal);
            await SaveChangesAsync();
            return animal;
        }

        /// <summary>
        /// Asynchronous updates information about specified animal in database
        /// </summary>
        /// <param name="animalId"></param>
        /// <param name="animalDto"></param>
        /// <returns>Updated animal data</returns>
        public async Task<Animal> UpdateAnimalAsync(int animalId, AnimalDto animalDto)
        {
            if (animalDto == null)
                return default!;

            var animalInDb = await _dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId
                                                                          && a.Deleted == false);

            if (animalInDb == null)
                return default!;
                                    
            animalInDb!.Name = animalDto!.Name;
            animalInDb!.Link = animalDto!.Link;
            animalInDb!.AnimalTypeId = animalDto!.AnimalTypeId;

            await SaveChangesAsync();

            return animalInDb;
        }

        /// <summary>
        /// Asynchronous deletes animal from database
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns>Deleted animal data</returns>
        public async Task<Animal> DeleteAnimalAsync(int animalId)
        {
            var animal = await _dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId);
            if (animal == null)
                return default!;

            animal.Deleted = true;
            await SaveChangesAsync();

            return animal;
        }

        /// <summary>
        /// Asynchronous returns information about animals which types matchs with specified
        /// </summary>
        /// <param name="animalTypeIds"></param>
        /// <returns>List of animals</returns>
        public async Task<List<Animal>> GetAnimalsByTypesAsync(int[] animalTypeIds)
        {
            var typesFromDb = _dbContext.AnimalTypes.Where(at => at.Deleted == false)
                                                    .Select(at => at.Id);
            var allTypesExistInDb = typesFromDb.All(x => animalTypeIds.Contains(x));

            if (allTypesExistInDb) return default!;

            return await _dbContext.Animals.Where(a => animalTypeIds.Contains(a.AnimalTypeId)
                                                  && a.Deleted == false)
                                           .ToListAsync(); ;
        }

        /// <summary>
        /// Asynchronous checks weather specified animal type in database or not
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>bool</returns>
        public async Task<bool> IsAnimalTypeExistAsync(int animalTypeId) =>
            await _dbContext.AnimalTypes.AnyAsync(t => t.Id == animalTypeId && t.Deleted == false);

        /// <summary>
        /// Asynchronous returns information about specified animal type from database
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>Animal type data</returns>
        public async Task<AnimalType> GetAnimalTypeAsync(int animalTypeId)
        {
            var animalType = await _dbContext.AnimalTypes.FirstOrDefaultAsync(t => 
                t.Id == animalTypeId && t.Deleted == false);
            if (animalType == null)
                return default!;

            return animalType;
        }

        /// <summary>
        /// Asynchronous adds new animal type to database
        /// </summary>
        /// <param name="animalType"></param>
        /// <returns>Added animal type data</returns>
        public async Task<AnimalType> AddAnimalTypeAsync(AnimalType animalType)
        {
            await _dbContext.AnimalTypes.AddAsync(animalType);
            await SaveChangesAsync();
            return animalType;
        }

        /// <summary>
        /// Asynchronous updates information about specified animal type in database
        /// </summary>
        /// <param name="animaltypeId"></param>
        /// <param name="animalTypeDto"></param>
        /// <returns>Updated animal type data</returns>
        public async Task<AnimalType> UpdateAnimalTypeAsync(int animaltypeId, AnimalTypeDto animalTypeDto)
        {
            if (animalTypeDto == null)
                return default!;
            
            var animalTypeInDb = await _dbContext.AnimalTypes.FirstOrDefaultAsync(a => 
            a.Id == animaltypeId && a.Deleted == false);

            if (animalTypeInDb == null)           
                return default!;

            animalTypeInDb!.Description = animalTypeDto!.Description;

            await SaveChangesAsync();

            return animalTypeInDb;
        }

        /// <summary>
        /// Asynchronous deletes animal type from database
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>Deleted animal type data</returns>
        public async Task<AnimalType> DeleteAnimalTypeAsync(int animalTypeId)
        {
            var animalType = await _dbContext.AnimalTypes.FirstOrDefaultAsync(a => 
                                                                              a.Id == animalTypeId);
            if (animalType == null)
                return default!;

            animalType.Deleted = true;
            await SaveChangesAsync();

            return animalType;
        }

        /// <summary>
        /// Asynchronous returns information about animal types which id matchs with specified
        /// </summary>
        /// <param name="animalTypesIds"></param>
        /// <returns>List of animal types</returns>
        public async Task<List<AnimalType>> GetAnimalTypesByIdsAsync(int[] animalTypesIds)
        {
            return await _dbContext.AnimalTypes
                .Where(t => animalTypesIds.Contains(t.Id) && t.Deleted == false).ToListAsync();
        }

        /// <summary>
        /// Saves changes to database
        /// </summary>
        /// <returns></returns>
        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}
