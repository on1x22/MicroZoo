using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Services
{
    /// <summary>
    /// Provides sending requests to another microservices if it's necessary.
    /// If sending not required then processing moves on to the next service
    /// </summary>
    public interface IAnimalTypesRequestReceivingService
    {
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
        Task<GetAnimalTypeResponse> UpdateAnimalTypeAsync(int animalTypeId, AnimalTypeDto animalTypeDto);

        /// <summary>
        /// Asynchronous deletes animal type from database
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <param name="accessToken"></param>
        /// <returns>GetAnimalTypeResponse with deleted animal type</returns>
        Task<GetAnimalTypeResponse> DeleteAnimalTypeAsync(int animalTypeId, string accessToken);

        /// <summary>
        /// Asynchronous returns information about animal types which id matchs with specified
        /// </summary>
        /// <param name="animalTypesIds"></param>
        /// <returns>GetAnimalTypesResponse</returns>
        Task<GetAnimalTypesResponse> GetAnimalTypesByIdsAsync([FromQuery] int[] animalTypesIds);
    }
}
