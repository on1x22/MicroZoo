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

        public async Task<GetSpecialityResponse> ChangeRelationBetweenZookeeperAndSpecialityAsync(int relationId, SpecialityDto specialityDto)
        {
            var response = new GetSpecialityResponse();

            var speciality = new Speciality();

            try
            {
                speciality = await _repository.ChangeRelationBetweenZookeeperAndSpecialityAsync(
                    relationId, specialityDto);
            }
            /*catch(BadRequestException ex)
            {
                response.ErrorMessage = $"Association between specified zookeeper and animal type " +
                    $"already exist";
            }*/
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.ErrorMessage = $"Association between specified zookeeper and animal type " +
                    $"already exist";
            }

            if(speciality.Id == 0 && response.ErrorMessage != null)
                return response;

            response.Speciality = speciality;
            return response;
        }
    }
}
