using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Services
{
    public interface IAnimalTypesRequestReceivingService
    {
        Task<GetAnimalTypesResponse> GetAllAnimalTypesAsync();
        Task<GetAnimalTypeResponse> GetAnimalTypeAsync(int animalTypeId);
        Task<GetAnimalTypeResponse> AddAnimalTypeAsync(AnimalTypeDto animalTypeDto);
        Task<GetAnimalTypeResponse> UpdateAnimalTypeAsync(int animalTypeId, AnimalTypeDto animalTypeDto);
        Task<GetAnimalTypeResponse> DeleteAnimalTypeAsync(int animalTypeId);
        Task<GetAnimalTypesResponse> GetAnimalTypesByIdsAsync([FromQuery] int[] animalTypesIds);
    }
}
