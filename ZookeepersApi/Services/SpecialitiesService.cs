using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.Infrastructure.Models.Specialities.Dto;
using MicroZoo.ZookeepersApi.Repository;

namespace MicroZoo.ZookeepersApi.Services
{
    public class SpecialitiesService : ISpecialitiesService
    {
        private readonly ISpecialitiesRepository _repository;
        private readonly ILogger _logger;

        public SpecialitiesService(ISpecialitiesRepository repository, ILogger<SpecialitiesService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Returns true, if one o more zokeepers with speciality exist in database
        /// </summary>
        /// <param name="checkType"></param>
        /// <param name="objectId"></param>
        /// <returns>True of false</returns>
        public async Task<CheckZokeepersWithSpecialityAreExistResponse>
            CheckZokeepersWithSpecialityAreExistAsync(CheckType checkType, int objectId)
        {
            var response = new CheckZokeepersWithSpecialityAreExistResponse();

            switch (checkType)
            {
                case CheckType.AnimalType:
                    response.IsThereZookeeperWithThisSpeciality = await _repository
                        .CheckZokeepersWithSpecialityAreExistAsync(objectId);
                    break;
                case CheckType.Person:
                    response.IsThereZookeeperWithThisSpeciality = await _repository
                        .CheckZookeeperIsExistAsync(objectId);
                    break;
            }

            /*if(response.IsThereZookeeperWithThisSpeciality)
            {
                response.ErrorMessage = $"There are zookeepers with specialization {animalTypeId}. " +
                    "Before deleting a specialty, you must remove the zookeepers " +
                    "association with that specialty.";
            }*/

            return response;
        }

        public async Task<GetSpecialityResponse> AddSpecialityAsync(SpecialityDto specialityDto)
        {
            var response = new GetSpecialityResponse();

            var speciality = new Speciality();

            try
            {
                speciality = await _repository.AddSpecialityAsync(specialityDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.ErrorMessage = $"Association between specified zookeeper and animal type " +
                    $"already exist";
            }

            if (speciality.Id == 0 && response.ErrorMessage != null)
                return response;

            response.Speciality = speciality;
            return response;
        }

        public async Task<GetSpecialityResponse> ChangeRelationBetweenZookeeperAndSpecialityAsync(int relationId, SpecialityDto specialityDto)
        {
            var response = new GetSpecialityResponse();

            var speciality = new Speciality();

            try
            {
                speciality = await _repository.ChangeRelationBetweenZookeeperAndSpecialityAsync(
                    relationId, specialityDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.ErrorMessage = $"Association between specified zookeeper and animal type " +
                    $"already exist";
            }

            if (speciality.Id == 0 && response.ErrorMessage != null)
                return response;

            response.Speciality = speciality;
            return response;
        }

        public async Task<GetSpecialitiesResponse> DeleteSpecialityAsync(SpecialityDto specialityDto)
        {
            var response = new GetSpecialitiesResponse();

            await _repository.DeleteSpecialityAsync(specialityDto);

            response.Specialities = await _repository
                .GetSpecialitiesByZookeeperIdAsync(specialityDto.ZookeeperId);

            if (response.Specialities == null)
                response.ErrorMessage = $"Speciealities for zookeeper " +
                    $"with id={specialityDto.ZookeeperId} are not exist";

            return response;
        }        
    }
}
