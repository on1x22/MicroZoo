using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Services
{
    /// <summary>
    /// Provides processing of animals and animal types requests
    /// </summary>
    public interface IAnimalsApiService
    {
        // Animals

        /// <summary>
        /// Asynchronous returns information about all animals from database
        /// </summary>
        /// <returns>GetAnimalsResponse</returns>
        Task<GetAnimalsResponse> GetAllAnimalsAsync();

        /// <summary>
        /// Asynchronous returns information about specified animal from database
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns>GetAnimalsResponse</returns>
        Task<GetAnimalResponse> GetAnimalAsync(int animalId);

        /// <summary>
        /// Asynchronous adds new animal to database
        /// </summary>
        /// <param name="animalDto"></param>
        /// <returns>GetAnimalResponse with added animal</returns>
        Task<GetAnimalResponse> AddAnimalAsync(AnimalDto animalDto);

        /// <summary>
        /// Asynchronous updates information about specified animal in database
        /// </summary>
        /// <param name="animalId"></param>
        /// <param name="animalDto"></param>
        /// <returns>GetAnimalResponse with updated animal</returns>
        Task<GetAnimalResponse> UpdateAnimalAsync(int animalId, AnimalDto animalDto);

        /// <summary>
        /// Asynchronous deletes animal from database
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns>GetAnimalResponse with deleted animal</returns>
        Task<GetAnimalResponse> DeleteAnimalAsync(int animalId);

        /// <summary>
        /// Asynchronous returns information about animals which types matchs with specified
        /// </summary>
        /// <param name="animalTypeIds"></param>
        /// <returns>GetAnimalsResponse with animals</returns>
        Task<GetAnimalsResponse> GetAnimalsByTypesAsync(int[] animalTypeIds);

        // AnimalTypes

        /// <summary>
        /// Asynchronous returns information about all animal types in database
        /// </summary>
        /// <returns>GetAnimalTypesResponse</returns>
        Task<GetAnimalTypesResponse> GetAllAnimalTypesAsync();

        /// <summary>
        /// Asynchronous returns information about specified animal type from database
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>GetAnimalTypeResponse</returns>
        Task<GetAnimalTypeResponse> GetAnimalTypeAsync(int animalTypeId);

        /// <summary>
        /// Asynchronous adds new animal type to database
        /// </summary>
        /// <param name="animalTypeDto"></param>
        /// <returns>GetAnimalTypeResponse with added animal type</returns>
        Task<GetAnimalTypeResponse> AddAnimalTypeAsync(AnimalTypeDto animalTypeDto);

        /// <summary>
        /// Asynchronous updates information about specified animal type in database
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <param name="animalTypeDto"></param>
        /// <returns>GetAnimalTypeResponse with updated animal type</returns>
        Task<GetAnimalTypeResponse> UpdateAnimalTypeAsync(int animalTypeId,
                                                          AnimalTypeDto animalTypeDto);

        /// <summary>
        /// Asynchronous deletes animal type from database
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>GetAnimalTypeResponse with deleted animal type</returns>
        Task<GetAnimalTypeResponse> DeleteAnimalTypeAsync(int animalTypeId);

        /// <summary>
        /// Asynchronous returns information about animal types which id matchs with specified
        /// </summary>
        /// <param name="animalTypesIds"></param>
        /// <returns>GetAnimalTypesResponse</returns>
        Task<GetAnimalTypesResponse> GetAnimalTypesByIdsAsync(int[] animalTypesIds);
    }
}
