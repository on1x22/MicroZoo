using MicroZoo.AnimalsApi.Repository;
using MicroZoo.Infrastructure.MassTransit.Responses;
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

        public async Task<GetAnimalResponse> AddAnimalAsync(AnimalDto animalDto)
        {
            var response = new GetAnimalResponse();
            if (!await _repository.IsAnimalTypeExist(animalDto.AnimalTypeId))
                response.ErrorMessage = "The specified animal type is not in the database";
            else
                response.Animal = await _repository.AddAnimalAsync(animalDto);
            
            return response;
        }

        public async Task<GetAllAnimalsResponse> GetAllAnimalsAsync()
        {
            var response = new GetAllAnimalsResponse();

            response.Animals = await _repository.GetAllAnimalsAsync();
            if (response.Animals == null)
                response.ErrorMessage = $"Database contains no entries";

            return response;
        }

        public async Task<GetAnimalResponse> GetAnimalAsync(int id)
        {
            var response = new GetAnimalResponse();

            response.Animal = await _repository.GetAnimalAsync(id);
            if (response.Animal == null)
                response.ErrorMessage = $"Animal with id = {id} not found";

            return response;
        }

        public async Task<GetAnimalResponse> UpdateAnimalAsync(int id, AnimalDto animalDto)
        {
            var response = new GetAnimalResponse();

            var animalFromDb = await _repository.GetAnimalAsync(id);

            if (animalFromDb == null)
            {
                response.ErrorMessage = $"Animal with id = {id} not found";
                return response;
            }

            if (!await _repository.IsAnimalTypeExist(animalDto.AnimalTypeId))
            {
                response.ErrorMessage = "The specified animal type is not in the database";
                return response;
            }

            response.Animal = await _repository.UpdateAnimalAsync(id, animalDto);
            return response;
        }

        public async Task<GetAnimalResponse> DeleteAnimalAsync(int id)
        {
            var response = new GetAnimalResponse();

            response.Animal = await _repository.DeleteAnimalAsync(id);
            if (response.Animal == null)
                response.ErrorMessage = $"Animal with id = {id} not found";

            return response;
        }
    }
}
