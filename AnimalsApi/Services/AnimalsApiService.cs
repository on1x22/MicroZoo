using MicroZoo.AnimalsApi.Repository;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Services
{
    public class AnimalsApiService : IAnimalsApiService
    {
        private readonly IAnimalRepository _repository;
        public AnimalsApiService(IAnimalRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAnimalAsync(Animal animal) =>        
            await _repository.AddAnimal(animal);            
        

        public async Task<List<Animal>> GetAllAnimalsAsync() =>
            await _repository.GetAllAnimalsAsync();

        public async Task<Animal> GetAnimalAsync(int id) =>
            await _repository.GetAnimal(id);

        public async Task<Animal> UpdateAnimalAsync(int id, AnimalDto animalDto) =>
            await _repository.UpdateAnimal(id, animalDto);
    }
}
