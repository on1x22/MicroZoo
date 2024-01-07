using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Animals.Dto;
using System.Runtime.CompilerServices;

namespace MicroZoo.AnimalsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnimalTypesController : ControllerBase
    {
        private readonly IServiceProvider _provider;
        private readonly Uri _rabbitMqUrl = new Uri("rabbitmq://localhost/animals-queue");

        public AnimalTypesController(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Get all animal types
        /// </summary>
        /// <returns>List of animal types</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAnimalTypes()
        {
            var response = await GetResponseFromRabbitTask<GetAllAnimalTypesRequest,
                GetAllAnimalTypesResponse>(new GetAllAnimalTypesRequest());

            return response.AnimalTypes != null
                ? Ok(response.AnimalTypes)
                : NotFound(response.ErrorMessage);
        }

        /// <summary>
        /// Get info about selected animal type
        /// </summary>
        /// <param name="animalTypeId"></param>
        /// <returns>c</returns>
        [HttpGet("{animalTypeId}")]
        public async Task<IActionResult> GetAnimalType(int animalTypeId)
        {
            var response = await GetResponseFromRabbitTask<GetAnimalTypeRequest, 
                GetAnimalTypeResponse>(new GetAnimalTypeRequest(animalTypeId));

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
            var response = await GetResponseFromRabbitTask<AddAnimalTypeRequest, 
                GetAnimalTypeResponse>(new AddAnimalTypeRequest(animalTypeDto));
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
            var response = await GetResponseFromRabbitTask<UpdateAnimalTypeRequest,
                GetAnimalTypeResponse>(new UpdateAnimalTypeRequest(animalTypeId, animalTypeDto));
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
            var response = await GetResponseFromRabbitTask<DeleteAnimalTypeRequest,
                GetAnimalTypeResponse>(new DeleteAnimalTypeRequest(animalTypeId));
            return response.AnimalType != null
                ? Ok(response.AnimalType)
                : NotFound(response.ErrorMessage);
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
