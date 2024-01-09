using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;

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
