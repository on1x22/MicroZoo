using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Repository
{
    public interface IAnimalRepository
    {
        // Animals
        Task<List<Animal>> GetAllAnimalsAsync();
        Task<Animal> GetAnimalAsync(int animalId);
        Task<List<Animal>> GetAnimalsByTypes(List<int> animalTypeIds);      // ??????
               
        
        Task<Animal> AddAnimalAsync(Animal animal);
        Task<Animal> UpdateAnimalAsync(int animalId, AnimalDto animalDto);
        Task<Animal> DeleteAnimalAsync(int animalId);
        Task<bool> IsAnimalTypeExist(int animalTypeId);
        Task<List<Animal>> GetAnimalsByTypesAsync(int[] animalTypeIds);         

        // AnimalTypes
        Task<List<AnimalType>> GetAllAnimalTypesAsync();
        Task<AnimalType> GetAnimalTypeAsync(int animalTypeId);
        Task<AnimalType> AddAnimalTypeAsync(AnimalType animalType);
        Task<AnimalType> UpdateAnimalTypeAsync(int animaltypeId, AnimalTypeDto animalTypeDto);
        Task<AnimalType> DeleteAnimalTypeAsync(int animalTypeId);
        Task<List<AnimalType>> GetAnimalTypesByIdsAsync(int[] animalTypesIds);
    }
}
