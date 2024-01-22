using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.Infrastructure.Models.Specialities.Dto;

namespace MicroZoo.ZookeepersApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SpecialitiesController : ControllerBase
    {
        private readonly IServiceProvider _provider;
        private readonly Uri _animalsApiUrl;
        private readonly Uri _personsApiUrl;
        private readonly Uri _zookeepersApiUrl;

        public SpecialitiesController(IServiceProvider provider, IConfiguration configuration)
        {
            _provider = provider;
            _animalsApiUrl = new Uri(configuration["ConnectionStrings:AnimalsApiRmq"]);
            _personsApiUrl = new Uri(configuration["ConnectionStrings:PersonsApiRmq"]);
            _zookeepersApiUrl = new Uri(configuration["ConnectionStrings:ZookeepersApiRmq"]);
        }

        /// <summary>
        /// Get all animal types
        /// </summary>
        /// <returns>List of animal types</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllSpecialities()
        {
            var response = await GetResponseFromRabbitTask<GetAllAnimalTypesRequest,
                GetAnimalTypesResponse>(new GetAllAnimalTypesRequest(), _animalsApiUrl);

            return response.AnimalTypes != null
                ? Ok(response.AnimalTypes)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Check that there is an association between specialty and any  zookeepers
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>True or false</returns>
        [HttpGet("animalTypes/{animalTypeId}")]
        public async Task<IActionResult> CheckZokeepersWithSpecialityAreExist(int animalTypeId)
        {
            var response = await
                GetResponseFromRabbitTask<CheckZokeepersWithSpecialityAreExistRequest,
                CheckZokeepersWithSpecialityAreExistResponse>
                (new CheckZokeepersWithSpecialityAreExistRequest(CheckType.AnimalType, animalTypeId), 
                _zookeepersApiUrl);

            return Ok(response.IsThereZookeeperWithThisSpeciality);
        }
         
        /// <summary>
        /// Check that zookeeper is associated with any specialty
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>True or false</returns>
        [HttpGet("zookeepers/{personId}")]
        public async Task<IActionResult> CheckZookeeperIsExist(int personId)
        {
            var response = await
                GetResponseFromRabbitTask<CheckZokeepersWithSpecialityAreExistRequest,
                CheckZokeepersWithSpecialityAreExistResponse>
                (new CheckZokeepersWithSpecialityAreExistRequest(CheckType.Person, personId),
                _zookeepersApiUrl);

            return Ok(response.IsThereZookeeperWithThisSpeciality);
        }

        /// <summary>
        /// Change relation between zookeeper and speciality
        /// </summary>
        /// <param name="relationId"></param>
        /// <param name="specialityDto"></param>
        /// <returns>Speciality</returns>
        [HttpPut("{relationId}")]
        public async Task<IActionResult> ChangeRelationBetweenZookeeperAndSpeciality(int relationId,
            [FromBody] SpecialityDto specialityDto)
        {
            var person = await GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                new GetPersonRequest(specialityDto.ZookeeperId), _personsApiUrl);

            var animalType = await GetResponseFromRabbitTask<GetAnimalTypeRequest,
                GetAnimalTypeResponse>(new GetAnimalTypeRequest(specialityDto.AnimalTypeId), _animalsApiUrl);

            string errorMessage = string.Empty;

            if (person.Person == null)
                errorMessage += person.ErrorMessage + ".\n";

            if (animalType.AnimalType == null)
                errorMessage += animalType.ErrorMessage;

            var response = new GetSpecialityResponse();

            if (errorMessage != string.Empty)
                return BadRequest(errorMessage);
                
            response = await GetResponseFromRabbitTask<
                ChangeRelationBetweenZookeeperAndSpecialityRequest, GetSpecialityResponse>(
                new ChangeRelationBetweenZookeeperAndSpecialityRequest(relationId, specialityDto), 
                _zookeepersApiUrl);

            return response.Speciality != null
                ? Ok(response.Speciality)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Delete the specialty specified in the request body
        /// </summary>
        /// <param name="specialityDto"></param>
        /// <returns>List of zookeeper specialities</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteSpeciality([FromBody] SpecialityDto specialityDto)
        {
            var specialities = await GetResponseFromRabbitTask<DeleteSpecialityRequest,
                GetSpecialitiesResponse>(new DeleteSpecialityRequest(specialityDto), _zookeepersApiUrl);

            if(specialities == null)
                return BadRequest(specialities.ErrorMessage);

            var animalTypesIds = specialities.Specialities.Select(x => x.AnimalTypeId).ToArray();

            var response = await GetResponseFromRabbitTask<GetAnimalTypesByIdsRequest,
                GetAnimalTypesResponse>(new GetAnimalTypesByIdsRequest(animalTypesIds), _animalsApiUrl);

            return response.AnimalTypes != null
            ? Ok(response.AnimalTypes)
            : BadRequest(response.ErrorMessage);
        }

        private async Task<TOut> GetResponseFromRabbitTask<TIn, TOut>(TIn request, Uri rabbitMqUrl)
            where TIn : class
            where TOut : class
        {
            var clientFactory = _provider.GetRequiredService<IClientFactory>();

            var client = clientFactory.CreateRequestClient<TIn>(rabbitMqUrl);
            var response = await client.GetResponse<TOut>(request);
            return response.Message;
        }
    }
}
