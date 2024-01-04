using MicroZoo.AnimalsApi.Repository;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.Services
{
    public class AnimalsApiService : IAnimalsApiService
    {
        private readonly IAnimalRepository _repository;
        public AnimalsApiService(IAnimalRepository repository)
        {
            this._repository = repository;
        }

        public async Task AddAnimalAsync(Animal animal)
        {
            await _repository.AddAnimal(animal);
            await _repository.SaveChangesAsync();
        }

        public async Task<List<Animal>> GetAllAnimalsAsync() =>
            await _repository.GetAllAnimalsAsync();

        public async Task<Animal> UpdateAnimalAsync(int id, Animal animal) =>
            await _repository.UpdateAnimal(id, animal);
    }
}
