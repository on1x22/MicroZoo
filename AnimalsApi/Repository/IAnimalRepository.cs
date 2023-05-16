using MicroZoo.AnimalsApi.Models;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.Repository
{
    public interface IAnimalRepository
    {
        Task<List<Animal>> GetAnimalsByTypes(List<int> animalTypeIds);
        Task<List<Animal>> GetAnimalsByTypes2(int[] animalTypeIds);
        Task<List<AnimalType>> GetAllAnimalTypes();
        Task<List<AnimalType>> GetAnimalTypesByIds(int[] animalTypeIds);
    }
}
