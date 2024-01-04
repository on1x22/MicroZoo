using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.Services
{
    public interface IAnimalsApiService
    {
        Task<List<Animal>> GetAllAnimalsAsync();
        Task AddAnimalAsync(Animal animal);
        Task<Animal> UpdateAnimalAsync(int id, Animal animal);
    }
}
