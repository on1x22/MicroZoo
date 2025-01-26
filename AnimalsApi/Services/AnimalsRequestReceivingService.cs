using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Services
{
    /// <summary>
    /// Provides sending requests to another microservices if it's necessary.
    /// If sending not required then processing moves on to the next service
    /// </summary>
    public class AnimalsRequestReceivingService : IAnimalsRequestReceivingService
    {
        private readonly IAnimalsApiService _animalsService;

        /// <summary>
        /// Initialize a new instance of <see cref="AnimalsRequestReceivingService"/> class
        /// </summary> 
        public AnimalsRequestReceivingService(IServiceProvider provider, 
            IAnimalsApiService animalsService,
            IConnectionService connectionService,
            IResponsesReceiverFromRabbitMq receiver)
        {
            _animalsService = animalsService;
        }

        /// <summary>
        /// Asynchronous returns information about all animals
        /// </summary>
        /// <returns>GetAnimalsResponse</returns>
        public async Task<GetAnimalsResponse> GetAllAnimalsAsync() =>        
             await _animalsService.GetAllAnimalsAsync();

        /// <summary>
        /// Asynchronous returns information about specified animal
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns>GetAnimalResponse</returns>
        public async Task<GetAnimalResponse> GetAnimalAsync(int animalId) =>        
            await _animalsService.GetAnimalAsync(animalId);

        /// <summary>
        /// Asynchronous adds new animal
        /// </summary>
        /// <param name="animalDto"></param>
        /// <returns>GetAnimalResponse with added animal</returns>
        public async Task<GetAnimalResponse> AddAnimalAsync(AnimalDto animalDto) =>
            await _animalsService.AddAnimalAsync(animalDto);

        /// <summary>
        /// Asynchronous updates information about specified animal
        /// </summary>
        /// <param name="animalId"></param>
        /// <param name="animalDto"></param>
        /// <returns>GetAnimalResponse with updated animal</returns>
        public async Task<GetAnimalResponse> UpdateAnimalAsync(int animalId, AnimalDto animalDto) =>
            await _animalsService.UpdateAnimalAsync(animalId, animalDto);

        /// <summary>
        /// Asynchronous deletes animal
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns>GetAnimalResponse with deleted animal</returns>
        public async Task<GetAnimalResponse> DeleteAnimalAsync(int animalId) =>
            await _animalsService.DeleteAnimalAsync(animalId);

        /// <summary>
        /// Asynchronous returns information about animals which types matchs with specified
        /// </summary>
        /// <param name="animalTypesIds"></param>
        /// <returns>GetAnimalsResponse with animals</returns>
        public async Task<GetAnimalsResponse> GetAnimalsByTypesAsync(int[] animalTypesIds) =>
            await _animalsService.GetAnimalsByTypesAsync(animalTypesIds);
    }
}
