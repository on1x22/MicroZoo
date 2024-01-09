using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Services
{
    public interface IAnimalsApiService
    {
        // Animals
        Task<GetAllAnimalsResponse> GetAllAnimalsAsync();
        Task<GetAnimalResponse> GetAnimalAsync(int animalId);
        Task<GetAnimalResponse> AddAnimalAsync(AnimalDto animalDto);
        Task<GetAnimalResponse> UpdateAnimalAsync(int animalId, AnimalDto animalDto);
        Task<GetAnimalResponse> DeleteAnimalAsync(int animalId);

        // AnimalTypes
        Task<GetAnimalTypesResponse> GetAllAnimalTypesAsync();
        Task<GetAnimalTypeResponse> GetAnimalTypeAsync(int animalTypeId);
        Task<GetAnimalTypeResponse> AddAnimalTypeAsync(AnimalTypeDto animalTypeDto);
        Task<GetAnimalTypeResponse> UpdateAnimalTypeAsync(int animalTypeId, AnimalTypeDto animalTypeDto);
        Task<GetAnimalTypeResponse> DeleteAnimalTypeAsync(int animalTypeId);
        Task<GetAnimalTypesResponse> GetAnimalTypesByIdsAsync(int[] animalTypesIds);
    }
}
