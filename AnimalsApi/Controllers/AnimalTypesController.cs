using Microsoft.AspNetCore.Mvc;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnimalTypesController : ControllerBase
    {
        //private readonly IServiceProvider _provider;
        private readonly IAnimalTypesRequestReceivingService _receivingService;

        //private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/animals-queue");
        //private readonly Uri _animalsApiUrl;
        //private readonly Uri _zookeepersApiUrl;

        public AnimalTypesController(/*IServiceProvider provider, IConfiguration configuration,*/
            IAnimalTypesRequestReceivingService receivingService)
        {
            //_provider = provider;
            _receivingService = receivingService;
            //_animalsApiUrl = new Uri(configuration["ConnectionStrings:AnimalsApiRmq"]);
            //_zookeepersApiUrl = new Uri(configuration["ConnectionStrings:ZookeepersApiRmq"]);
        }

        /// <summary>
        /// Get all animal types
        /// </summary>
        /// <returns>List of animal types</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAnimalTypes()
        {
            //var response = await GetResponseFromRabbitTask<GetAllAnimalTypesRequest,
            //    GetAnimalTypesResponse>(new GetAllAnimalTypesRequest(), _animalsApiUrl);
            var response = await _receivingService.GetAllAnimalTypesAsync();

            return response.AnimalTypes != null
                ? Ok(response.AnimalTypes)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Get info about selected animal type
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>Animal type info</returns>
        [HttpGet("{animalTypeId}")]
        public async Task<IActionResult> GetAnimalType(int animalTypeId)
        {
            //var response = await GetResponseFromRabbitTask<GetAnimalTypeRequest, 
            //    GetAnimalTypeResponse>(new GetAnimalTypeRequest(animalTypeId), _animalsApiUrl);
            var response = await _receivingService.GetAnimalTypeAsync(animalTypeId);

            return response.AnimalType != null
                ? Ok(response.AnimalType)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Create new animal type
        /// </summary>
        /// <param name="animalTypeDto"></param>
        /// <returns>Created animal type</returns>
        [HttpPost]
        public async Task<IActionResult> AddAnimalType([FromBody] AnimalTypeDto animalTypeDto)
        {
            //var response = await GetResponseFromRabbitTask<AddAnimalTypeRequest, 
            //    GetAnimalTypeResponse>(new AddAnimalTypeRequest(animalTypeDto), _animalsApiUrl);
            var response = await _receivingService.AddAnimalTypeAsync(animalTypeDto);
            
            return response.AnimalType != null
                ? Ok(response.AnimalType)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Change data about selected animal type
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <param name="animalTypeDto"></param>
        /// <returns>Changed info about selected animal type</returns>
        [HttpPut("{animalTypeId}")]
        public async Task<IActionResult> UpdateAnimalType(int animalTypeId, 
            [FromBody] AnimalTypeDto animalTypeDto)
        {
            //var response = await GetResponseFromRabbitTask<UpdateAnimalTypeRequest,
            //    GetAnimalTypeResponse>(new UpdateAnimalTypeRequest(animalTypeId, animalTypeDto), 
            //                                                       _animalsApiUrl);
            var response = await _receivingService.UpdateAnimalTypeAsync(animalTypeId, animalTypeDto);
            
            return response.AnimalType != null
                ? Ok(response.AnimalType)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Delete selected animal type
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>Deleted animal type</returns>
        [HttpDelete("{animalTypeId}")]
        public async Task<IActionResult> DeleteAnimalType(int animalTypeId)
        {
            /*// This action is in question. This check should be performed
            // in the upstream microservice
            var isThereZokeeperWithSpecialty = await
                GetResponseFromRabbitTask<CheckZokeepersWithSpecialityAreExistRequest, 
                CheckZokeepersWithSpecialityAreExistResponse>
                (new CheckZokeepersWithSpecialityAreExistRequest(CheckType.AnimalType, animalTypeId, null!), 
                _zookeepersApiUrl);

            if (isThereZokeeperWithSpecialty.IsThereZookeeperWithThisSpeciality)
                return BadRequest($"There are zookeepers with specialization {animalTypeId}. " +
                    "Before deleting a specialty, you must remove the zookeepers " +
                    "association with that specialty.");*/

            //var response = await GetResponseFromRabbitTask<DeleteAnimalTypeRequest,
            //    GetAnimalTypeResponse>(new DeleteAnimalTypeRequest(animalTypeId), _animalsApiUrl);
            var response = await _receivingService.DeleteAnimalTypeAsync(animalTypeId);
            
            return response.AnimalType != null
                ? Ok(response.AnimalType)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Get animal types by selected Ids
        /// </summary>
        /// <param name="animalTypesIds)"></param>
        /// <returns>List of selected animal types</returns>
        [HttpGet("byIds")]
        public async Task<IActionResult> GetAnimalTypesByIds([FromQuery] int[] animalTypesIds)
        {
            //var response = await GetResponseFromRabbitTask<GetAnimalTypesByIdsRequest,
            //    GetAnimalTypesResponse>(new GetAnimalTypesByIdsRequest(animalTypesIds), _animalsApiUrl);
            var response = await _receivingService.GetAnimalTypesByIdsAsync(animalTypesIds);
            
            return response.AnimalTypes != null
            ? Ok(response.AnimalTypes)
            : BadRequest(response.ErrorMessage); 
        }

        /*private async Task<TOut> GetResponseFromRabbitTask<TIn, TOut>(TIn request, Uri rabbitMqUrl)
            where TIn : class
            where TOut : class
        {
            var clientFactory = _provider.GetRequiredService<IClientFactory>();

            var client = clientFactory.CreateRequestClient<TIn>(rabbitMqUrl);
            var response = await client.GetResponse<TOut>(request);
            return response.Message;
        }*/
    }
}
