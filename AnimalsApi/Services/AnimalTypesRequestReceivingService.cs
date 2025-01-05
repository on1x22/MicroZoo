using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Services
{
    public class AnimalTypesRequestReceivingService : IAnimalTypesRequestReceivingService
    {
        private readonly IAnimalsApiService _animalsService;

        public AnimalTypesRequestReceivingService(IAnimalsApiService animalsService)
        {
            _animalsService = animalsService;
        }

        public async Task<GetAnimalTypesResponse> GetAllAnimalTypesAsync() =>
            await _animalsService.GetAllAnimalTypesAsync();       

        public async Task<GetAnimalTypeResponse> GetAnimalTypeAsync(int animalTypeId) =>
            await _animalsService.GetAnimalTypeAsync(animalTypeId);

        public async Task<GetAnimalTypeResponse> AddAnimalTypeAsync(AnimalTypeDto animalTypeDto) =>
            await _animalsService.AddAnimalTypeAsync(animalTypeDto);
        
        public async Task<GetAnimalTypeResponse> UpdateAnimalTypeAsync(int animalTypeId, 
            AnimalTypeDto animalTypeDto) =>
            await _animalsService.UpdateAnimalTypeAsync(animalTypeId, animalTypeDto);

        public async Task<GetAnimalTypeResponse> DeleteAnimalTypeAsync(int animalTypeId) =>
            await _animalsService.DeleteAnimalTypeAsync(animalTypeId);
        
        public async Task<GetAnimalTypesResponse> GetAnimalTypesByIdsAsync(int[] animalTypesIds) =>
            await _animalsService.GetAnimalTypesByIdsAsync(animalTypesIds);
       

        
    }
}
