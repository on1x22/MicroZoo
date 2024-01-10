using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.AnimalsApi.Services;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;

namespace MicroZoo.AnimalsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IServiceProvider _provider;
        private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/animals-queue");

        public AnimalsController(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Get all animals
        /// </summary>
        /// <returns>List of animals</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAnimals(/*IAnimalsApiService service*/)
        {
            var response = await GetResponseFromRabbitTask<GetAllAnimalsRequest, GetAnimalsResponse>(new GetAllAnimalsRequest());
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
            var response = await GetResponseFromRabbitTask<GetAnimalRequest, GetAnimalResponse>(new GetAnimalRequest(animalId));

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
            var response = await GetResponseFromRabbitTask<AddAnimalRequest, GetAnimalResponse>(new AddAnimalRequest(animalDto));
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
            var response = await GetResponseFromRabbitTask<UpdateAnimalRequest, GetAnimalResponse>(new UpdateAnimalRequest(animalId, animalDto));
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
            var response = await GetResponseFromRabbitTask<DeleteAnimalRequest, GetAnimalResponse>(new DeleteAnimalRequest(animalId));
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
            var response = await GetResponseFromRabbitTask<GetAnimalsByTypesRequest,
                GetAnimalsResponse>(new GetAnimalsByTypesRequest(animalTypesIds));
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
