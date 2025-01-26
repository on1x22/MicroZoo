using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Repository
{
    /// <summary>
    /// Provides processing of animals and animal types requests to database
    /// </summary>
    public interface IAnimalRepository
    {
        // Animals

        /// <summary>
        /// Asynchronous returns information about all animals from database
        /// </summary>
        /// <returns>List of animals</returns>
        Task<List<Animal>> GetAllAnimalsAsync();

        /// <summary>
        /// Asynchronous returns information about specified animal from database
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns>Animal data</returns>
        Task<Animal> GetAnimalAsync(int animalId);

        /// <summary>
        /// Asynchronous returns information about animals which types matchs with specified
        /// </summary>
        /// <param name="animalTypeIds"></param>
        /// <returns>List of animals</returns>
        Task<List<Animal>> GetAnimalsByTypesAsync(List<int> animalTypeIds);      // ??????

        /// <summary>
        /// Asynchronous adds new animal to database
        /// </summary>
        /// <param name="animal"></param>
        /// <returns>Added animal data</returns>
        Task<Animal> AddAnimalAsync(Animal animal);

        /// <summary>
        /// Asynchronous updates information about specified animal in database
        /// </summary>
        /// <param name="animalId"></param>
        /// <param name="animalDto"></param>
        /// <returns>Updated animal data</returns>
        Task<Animal> UpdateAnimalAsync(int animalId, AnimalDto animalDto);

        /// <summary>
        /// Asynchronous deletes animal from database
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns>Deleted animal data</returns>
        Task<Animal> DeleteAnimalAsync(int animalId);

        /// <summary>
        /// Asynchronous checks weather specified animal type in database or not
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>bool</returns>
        Task<bool> IsAnimalTypeExistAsync(int animalTypeId);

        /// <summary>
        /// Asynchronous returns information about animals which types matchs with specified
        /// </summary>
        /// <param name="animalTypeIds"></param>
        /// <returns>List of animals</returns>
        Task<List<Animal>> GetAnimalsByTypesAsync(int[] animalTypeIds);

        // AnimalTypes

        /// <summary>
        /// Asynchronous returns information about all animal types in database
        /// </summary>
        /// <returns>List of animal types</returns>
        Task<List<AnimalType>> GetAllAnimalTypesAsync();

        /// <summary>
        /// Asynchronous returns information about specified animal type from database
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>Animal type data</returns>
        Task<AnimalType> GetAnimalTypeAsync(int animalTypeId);

        /// <summary>
        /// Asynchronous adds new animal type to database
        /// </summary>
        /// <param name="animalType"></param>
        /// <returns>Added animal type data</returns>
        Task<AnimalType> AddAnimalTypeAsync(AnimalType animalType);

        /// <summary>
        /// Asynchronous updates information about specified animal type in database
        /// </summary>
        /// <param name="animaltypeId"></param>
        /// <param name="animalTypeDto"></param>
        /// <returns>Updated animal type data</returns>
        Task<AnimalType> UpdateAnimalTypeAsync(int animaltypeId, AnimalTypeDto animalTypeDto);

        /// <summary>
        /// Asynchronous deletes animal type from database
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>Deleted animal type data</returns>
        Task<AnimalType> DeleteAnimalTypeAsync(int animalTypeId);

        /// <summary>
        /// Asynchronous returns information about animal types which id matchs with specified
        /// </summary>
        /// <param name="animalTypesIds"></param>
        /// <returns>List of animal types</returns>
        Task<List<AnimalType>> GetAnimalTypesByIdsAsync(int[] animalTypesIds);
    }
}
