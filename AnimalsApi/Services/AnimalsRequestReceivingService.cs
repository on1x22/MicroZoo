using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Services
{
    public class AnimalsRequestReceivingService : IAnimalsRequestReceivingService
    {
        private readonly IAnimalsApiService _animalsService;

        public AnimalsRequestReceivingService(IServiceProvider provider, 
            IAnimalsApiService animalsService,
            IConnectionService connectionService,
            IResponsesReceiverFromRabbitMq receiver)
        {
            _animalsService = animalsService;
        }

        public async Task<GetAnimalsResponse> GetAllAnimalsAsync() =>        
             await _animalsService.GetAllAnimalsAsync();
        
        public async Task<GetAnimalResponse> GetAnimalAsync(int animalId) =>        
            await _animalsService.GetAnimalAsync(animalId);
        
        public async Task<GetAnimalResponse> AddAnimalAsync(AnimalDto animalDto) =>
            await _animalsService.AddAnimalAsync(animalDto);        

        public async Task<GetAnimalResponse> UpdateAnimalAsync(int animalId, AnimalDto animalDto) =>
            await _animalsService.UpdateAnimalAsync(animalId, animalDto);

        public async Task<GetAnimalResponse> DeleteAnimalAsync(int animalId) =>
            await _animalsService.DeleteAnimalAsync(animalId);

        public async Task<GetAnimalsResponse> GetAnimalsByTypesAsync(int[] animalTypesIds) =>
            await _animalsService.GetAnimalsByTypesAsync(animalTypesIds);
    }
}
