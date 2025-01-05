using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace AnimalsApi.Services
{
    public interface IAnimalsRequestReceivingService
    {
        Task<GetAnimalsResponse> GetAllAnimalsAsync ();
        Task<GetAnimalResponse> GetAnimalAsync(int animalId);
        Task<GetAnimalResponse> AddAnimalAsync(AnimalDto animalDto);
        Task<GetAnimalResponse> UpdateAnimalAsync(int animalId, AnimalDto animalDto);
        Task<GetAnimalResponse> DeleteAnimalAsync(int animalId);
        Task<GetAnimalsResponse> GetAnimalsByTypesAsync(int[] animalTypesIds);
    }
}
