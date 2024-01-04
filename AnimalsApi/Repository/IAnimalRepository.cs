using MicroZoo.AnimalsApi.Models;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Repository
{
    public interface IAnimalRepository
    {
        Task<List<Animal>> GetAllAnimalsAsync();
        Task<Animal> GetAnimalAsync(int id);
        Task<List<Animal>> GetAnimalsByTypes(List<int> animalTypeIds);
        Task<List<Animal>> GetAnimalsByTypes2(int[] animalTypeIds);
        Task<List<AnimalType>> GetAllAnimalTypes();
        Task<List<AnimalType>> GetAnimalTypesByIds(int[] animalTypeIds);
        Task AddAnimalAsync(Animal animal);
        Task<Animal> UpdateAnimalAsync(int id, AnimalDto animalDto);
        Task<Animal> DeleteAnimalAsync(int id);
    }
}
