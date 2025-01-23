using MicroZoo.AnimalsApi.Repository;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
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

        public async Task<GetAnimalsResponse> GetAllAnimalsAsync()
        {
            var response = new GetAnimalsResponse
            {
                Animals = await _repository.GetAllAnimalsAsync()
            };

            if (response.Animals == null)
            {
                response.ErrorMessage = $"Database contains no entries";
                response.ErrorCode = ErrorCodes.NotFound404;
            }

            return response;
        }
        
        public async Task<GetAnimalResponse> GetAnimalAsync(int animalId)
        {
            var response = new GetAnimalResponse
            {
                Animal = await _repository.GetAnimalAsync(animalId)
            };

            if (response.Animal == null)
            {
                response.ErrorMessage = $"Animal with id = {animalId} not found";
                response.ErrorCode = ErrorCodes.NotFound404;
            }

            return response;
        }

        public async Task<GetAnimalResponse> AddAnimalAsync(AnimalDto animalDto)
        {
            var response = new GetAnimalResponse();
            if (!await _repository.IsAnimalTypeExist(animalDto.AnimalTypeId))
            {
                response.ErrorMessage = "The specified animal type is not in the database";
                response.ErrorCode = ErrorCodes.BadRequest400;
                return response;
            }

            var addedAnimal = AnimalDto.DtoToAnimal(animalDto);
            response.Animal = await _repository.AddAnimalAsync(addedAnimal);
            
            return response;
        }
             
        public async Task<GetAnimalResponse> UpdateAnimalAsync(int animalId, AnimalDto animalDto)
        {
            var response = new GetAnimalResponse();

            var animalFromDb = await _repository.GetAnimalAsync(animalId);

            if (animalFromDb == null)
            {
                response.ErrorMessage = $"Animal with id = {animalId} not found";
                response.ErrorCode = ErrorCodes.NotFound404;
                return response;
            }

            if (!await _repository.IsAnimalTypeExist(animalDto.AnimalTypeId))
            {
                response.ErrorMessage = "The specified animal type is not in the database";
                response.ErrorCode = ErrorCodes.BadRequest400;
                return response;
            }

            response.Animal = await _repository.UpdateAnimalAsync(animalId, animalDto);
            if (response.Animal == null)
            {
                response.ErrorMessage = $"Animal with id = {animalId} not found";
                response.ErrorCode = ErrorCodes.NotFound404;
            }

            return response;
        }

        public async Task<GetAnimalResponse> DeleteAnimalAsync(int animalId)
        {
            var response = new GetAnimalResponse
            {
                Animal = await _repository.DeleteAnimalAsync(animalId)
            };

            if (response.Animal == null)
            {
                response.ErrorMessage = $"Animal with id = {animalId} not found";
                response.ErrorCode = Infrastructure.MassTransit.ErrorCodes.NotFound404;
            }

            return response;
        }

        public async Task<GetAnimalsResponse> GetAnimalsByTypesAsync(int[] animalTypeIds)
        {
            var response = new GetAnimalsResponse
            {
                Animals = await _repository.GetAnimalsByTypesAsync(animalTypeIds)
            };

            if (response.Animals == null)
            {
                response.ErrorMessage = "Not all animal type Ids exist in database";
                response.ErrorCode = ErrorCodes.BadRequest400;
            }

            return response;
        }

        public async Task<GetAnimalTypesResponse> GetAllAnimalTypesAsync()
        {
            var response = new GetAnimalTypesResponse
            {
                AnimalTypes = await _repository.GetAllAnimalTypesAsync()
            };

            if (response.AnimalTypes == null)
            {
                response.ErrorMessage = $"Database contains no entries";
                response.ErrorCode = ErrorCodes.NotFound404;
            }

            return response;
        }

        public async Task<GetAnimalTypeResponse> GetAnimalTypeAsync(int animalTypeId)
        {
            var response = new GetAnimalTypeResponse
            {
                AnimalType = await _repository.GetAnimalTypeAsync(animalTypeId)
            };

            if (response.AnimalType == null)
            {
                response.ErrorMessage = $"Animal type with id = {animalTypeId} not found";
                response.ErrorCode = ErrorCodes.NotFound404;
            }

            return response;
        }

        public async Task<GetAnimalTypeResponse> AddAnimalTypeAsync(AnimalTypeDto animalTypeDto)
        {
            var response = new GetAnimalTypeResponse();

            if (animalTypeDto == null)
            {
                response.ErrorMessage = "Invalid data entered";
                response.ErrorCode = ErrorCodes.BadRequest400;
                return response;
            }
            
            var addedAnimalType = AnimalTypeDto.DtoToAnimalType(animalTypeDto);
            response.AnimalType = await _repository.AddAnimalTypeAsync(addedAnimalType);

            if (response.AnimalType == null)
            {
                response.ErrorMessage = "Error when creating object in database";
                response.ErrorCode = ErrorCodes.InternalServerError500;
            }
            return response;
        }

        public async Task<GetAnimalTypeResponse> UpdateAnimalTypeAsync(int animalTypeId, AnimalTypeDto animalTypeDto)
        {
            var response = new GetAnimalTypeResponse();

            var animalTypeFromDb = await _repository.GetAnimalTypeAsync(animalTypeId);

            if (animalTypeFromDb == null)
            {
                response.ErrorMessage = $"Animal type with id = {animalTypeId} not found";
                response.ErrorCode = ErrorCodes.BadRequest400;
                return response;
            }

            response.AnimalType = await _repository.UpdateAnimalTypeAsync(animalTypeId, animalTypeDto);
            return response;
        }

        public async Task<GetAnimalTypeResponse> DeleteAnimalTypeAsync(int animalTypeId)
        {
            var response = new GetAnimalTypeResponse
            {
                AnimalType = await _repository.DeleteAnimalTypeAsync(animalTypeId)
            };

            if (response.AnimalType == null)
            {
                response.ErrorMessage = $"Animal type with id = {animalTypeId} not found";
                response.ErrorCode = ErrorCodes.NotFound404;
            }
            return response;
        }

        public async Task<GetAnimalTypesResponse> GetAnimalTypesByIdsAsync(int[] animalTypesIds)
        {
            var response = new GetAnimalTypesResponse
            {
                AnimalTypes = await _repository.GetAnimalTypesByIdsAsync(animalTypesIds)
            };

            if (response.AnimalTypes == null || response.AnimalTypes.Count != animalTypesIds.Length)
            {
                response.ErrorMessage = $"Not all animal types are found in database";
                response.ErrorCode = ErrorCodes.NotFound404;
                response.AnimalTypes = null;
            }

            return response;
        }
    }
}
