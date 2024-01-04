using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Services
{
    public interface IAnimalsApiService
    {
        Task<List<Animal>> GetAllAnimalsAsync();
        Task<Animal> GetAnimalAsync(int id);
        Task AddAnimalAsync(Animal animal);
        Task<Animal> UpdateAnimalAsync(int id, AnimalDto animalDto);
        Task<Animal> DeleteAnimalAsync(int id);
    }
}
