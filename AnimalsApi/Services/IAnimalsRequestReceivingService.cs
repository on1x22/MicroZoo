using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Services
{
    /// <summary>
    /// Provides sending requests to another microservices if it's necessary.
    /// If sending not required then processing moves on to the next service
    /// </summary>
    public interface IAnimalsRequestReceivingService
    {
        /// <summary>
        /// Asynchronous returns information about all animals
        /// </summary>
        /// <returns>GetAnimalsResponse</returns>
        Task<GetAnimalsResponse> GetAllAnimalsAsync ();

        /// <summary>
        /// Asynchronous returns information about specified animal
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns>GetAnimalResponse</returns>
        Task<GetAnimalResponse> GetAnimalAsync(int animalId);

        /// <summary>
        /// Asynchronous adds new animal
        /// </summary>
        /// <param name="animalDto"></param>
        /// <returns>GetAnimalResponse with added animal</returns>
        Task<GetAnimalResponse> AddAnimalAsync(AnimalDto animalDto);

        /// <summary>
        /// Asynchronous updates information about specified animal
        /// </summary>
        /// <param name="animalId"></param>
        /// <param name="animalDto"></param>
        /// <returns>GetAnimalResponse with updated animal</returns>
        Task<GetAnimalResponse> UpdateAnimalAsync(int animalId, AnimalDto animalDto);

        /// <summary>
        /// Asynchronous deletes animal
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns>GetAnimalResponse with deleted animal</returns>
        Task<GetAnimalResponse> DeleteAnimalAsync(int animalId);

        /// <summary>
        /// Asynchronous returns information about animals which types matchs with specified
        /// </summary>
        /// <param name="animalTypesIds"></param>
        /// <returns>GetAnimalsResponse with animals</returns>
        Task<GetAnimalsResponse> GetAnimalsByTypesAsync(int[] animalTypesIds);
    }
}
