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

        public async Task<GetAllAnimalsResponse> GetAllAnimalsAsync()
        {
            var response = new GetAllAnimalsResponse();

            response.Animals = await _repository.GetAllAnimalsAsync();
            if (response.Animals == null)
                response.ErrorMessage = $"Database contains no entries";

            return response;
        }
        
        public async Task<GetAnimalResponse> GetAnimalAsync(int animalId)
        {
            var response = new GetAnimalResponse();

            response.Animal = await _repository.GetAnimalAsync(animalId);
            if (response.Animal == null)
                response.ErrorMessage = $"Animal with id = {animalId} not found";

            return response;
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
             
        public async Task<GetAnimalResponse> UpdateAnimalAsync(int animalId, AnimalDto animalDto)
        {
            var response = new GetAnimalResponse();

            var animalFromDb = await _repository.GetAnimalAsync(animalId);

            if (animalFromDb == null)
            {
                response.ErrorMessage = $"Animal with id = {animalId} not found";
                return response;
            }

            if (!await _repository.IsAnimalTypeExist(animalDto.AnimalTypeId))
            {
                response.ErrorMessage = "The specified animal type is not in the database";
                return response;
            }

            response.Animal = await _repository.UpdateAnimalAsync(animalId, animalDto);
            return response;
        }

        public async Task<GetAnimalResponse> DeleteAnimalAsync(int animalId)
        {
            var response = new GetAnimalResponse();

            response.Animal = await _repository.DeleteAnimalAsync(animalId);
            if (response.Animal == null)
                response.ErrorMessage = $"Animal with id = {animalId} not found";

            return response;
        }

        public async Task<GetAllAnimalTypesResponse> GetAllAnimalTypesAsync()
        {
            var response = new GetAllAnimalTypesResponse();

            response.AnimalTypes = await _repository.GetAllAnimalTypesAsync();
            if (response.AnimalTypes == null)
                response.ErrorMessage = $"Database contains no entries";

            return response;
        }

        public async Task<GetAnimalTypeResponse> GetAnimalTypeAsync(int animalTypeId)
        {
            var response = new GetAnimalTypeResponse();

            response.AnimalType = await _repository.GetAnimalTypeAsync(animalTypeId);
            if (response.AnimalType == null)
                response.ErrorMessage = $"Animal type with id = {animalTypeId} not found";

            return response;
        }

        public async Task<GetAnimalTypeResponse> AddAnimalTypeAsync(AnimalTypeDto animalTypeDto)
        {
            var response = new GetAnimalTypeResponse();

            if (animalTypeDto == null)
                response.ErrorMessage = "Invalid data entered";
            else
                response.AnimalType = await _repository.AddAnimalTypeAsync(animalTypeDto);

            if (response.AnimalType == null)
                response.ErrorMessage = "Error when creating object in database";

            return response;
        }

        public async Task<GetAnimalTypeResponse> UpdateAnimalTypeAsync(int animalTypeId, AnimalTypeDto animalTypeDto)
        {
            var response = new GetAnimalTypeResponse();

            var animalTypeFromDb = await _repository.GetAnimalTypeAsync(animalTypeId);

            if (animalTypeFromDb == null)
            {
                response.ErrorMessage = $"Animal type with id = {animalTypeId} not found";
                return response;
            }

            response.AnimalType = await _repository.UpdateAnimalTypeAsync(animalTypeId, animalTypeDto);
            return response;
        }

        public async Task<GetAnimalTypeResponse> DeleteAnimalTypeAsync(int animalTypeId)
        {
            var response = new GetAnimalTypeResponse();

            response.AnimalType = await _repository.DeleteAnimalTypeAsync(animalTypeId);
            if (response.AnimalType == null)
                response.ErrorMessage = $"Animal type with id = {animalTypeId} not found";

            return response;
        }
    }
}
