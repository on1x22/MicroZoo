using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.Services
{
    public interface IAnimalsApiService
    {
        Task<List<Animal>> GetAllAnimalsAsync();
        Task AddAnimal(Animal animal);
    }
}
