using MicroZoo.Infrastructure.MassTransit.Responses;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Services
{
    public interface IAnimalsApiService
    {
        Task<GetAllAnimalsResponse> GetAllAnimalsAsync();
        Task<GetAnimalResponse> GetAnimalAsync(int id);
        Task<GetAnimalResponse> AddAnimalAsync(AnimalDto animalDto);
        Task<GetAnimalResponse> UpdateAnimalAsync(int id, AnimalDto animalDto);
        Task<GetAnimalResponse> DeleteAnimalAsync(int id);
    }
}
