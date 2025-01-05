using AnimalsApi.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Controllers
{
    /// <summary>
    /// Controller for handling animals requests
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IServiceProvider _provider;
        private readonly IAnimalsRequestReceivingService _receivingService;
        private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/animals-queue");

        /// <summary>
        /// Initialize a new instance of <see cref="AnimalsController"/> class
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="receivingService"></param>
        public AnimalsController(IServiceProvider provider,
            IAnimalsRequestReceivingService receivingService)
        {
            _provider = provider;
            _receivingService = receivingService;
        }

        /// <summary>
        /// Get all animals
        /// </summary>
        /// <returns>List of animals</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAnimals()
        {
            //var response = await GetResponseFromRabbitTask<GetAllAnimalsRequest, GetAnimalsResponse>(new GetAllAnimalsRequest());
            var response = await _receivingService.GetAllAnimalsAsync();
            
            return response.Animals != null
                ? Ok(response.Animals)
                : NoContent();
        }

        /// <summary>
        /// Get info about selected animal
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns>Animal info</returns>
        [HttpGet("{animalId}")]
        public async Task<IActionResult> GetAnimal(int animalId)
        {
            //var response = await GetResponseFromRabbitTask<GetAnimalRequest, GetAnimalResponse>(new GetAnimalRequest(animalId));
            var response = await _receivingService.GetAnimalAsync(animalId);

            return response.Animal != null
                ? Ok(response.Animal)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Create new animal
        /// </summary>
        /// <param name="animalDto"></param>
        /// <returns>Created animal</returns>
        [HttpPost]
        public async Task<IActionResult> AddAnimal([FromBody] AnimalDto animalDto)
        {
            //var response = await GetResponseFromRabbitTask<AddAnimalRequest, GetAnimalResponse>(new AddAnimalRequest(animalDto));
            var response = await _receivingService.AddAnimalAsync(animalDto);

            return response.Animal != null
                ? Ok(response.Animal)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Change data about selected animal
        /// </summary>
        /// <param name="animalId"></param>
        /// <param name="animalDto"></param>
        /// <returns>Changed info about selected animal</returns>
        [HttpPut("{animalId}")]
        public async Task<IActionResult> UpdateAnimal(int animalId, [FromBody] AnimalDto animalDto)
        {
            //var response = await GetResponseFromRabbitTask<UpdateAnimalRequest, GetAnimalResponse>(new UpdateAnimalRequest(animalId, animalDto));
            var response = await _receivingService.UpdateAnimalAsync(animalId, animalDto);

            return response.Animal != null
                ? Ok(response.Animal)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Delete selected animal
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns>Deleted animal</returns>
        [HttpDelete("{animalId}")]
        public async Task<IActionResult> DeleteAnimal(int animalId)
        {
            //var response = await GetResponseFromRabbitTask<DeleteAnimalRequest, GetAnimalResponse>(new DeleteAnimalRequest(animalId));
            var response = await _receivingService.DeleteAnimalAsync(animalId);
            
            return response.Animal != null
                ? Ok(response.Animal)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Get animals by selected animal types Ids
        /// </summary>
        /// <param name="animalTypesIds)"></param>
        /// <returns>List of animals by selected animal types</returns>
        [HttpGet("byTypes")]
        public async Task<IActionResult> GetAnimalsByTypes([FromQuery] int[] animalTypesIds)
        {
            //var response = await GetResponseFromRabbitTask<GetAnimalsByTypesRequest,
            //    GetAnimalsResponse>(new GetAnimalsByTypesRequest(animalTypesIds));
            var response = await _receivingService.GetAnimalsByTypesAsync(animalTypesIds);
            
            return response.Animals != null
            ? Ok(response.Animals)
            : BadRequest(response.ErrorMessage);
        }

        private async Task<TOut> GetResponseFromRabbitTask<TIn, TOut>(TIn request)
            where TIn : class
            where TOut : class
        {
            var clientFactory = _provider.GetRequiredService<IClientFactory>();

            var client = clientFactory.CreateRequestClient<TIn>(_rabbitMqUrl);
            var response = await client.GetResponse<TOut>(request);
            return response.Message;
        }
    }
}
