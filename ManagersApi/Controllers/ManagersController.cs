using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Requests;
using MicroZoo.Infrastructure.MassTransit.Responses;
using MicroZoo.Infrastructure.Models.Animals;


namespace MicroZoo.ManagersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly IServiceProvider _provider;
        private readonly Uri _animalsApiRmqUrl = new Uri("rabbitmq://localhost/animals-queue");

        public ManagersController(IServiceProvider provider)
        {
            _provider = provider;
        }

        [HttpGet("animals")]
        public async Task<IActionResult> GetAllAnimals()
        {
            var response = await GetResponseFromRabbitTask<GetAllAnimalsRequest,
                GetAllAnimalsResponse>(_animalsApiRmqUrl, new GetAllAnimalsRequest());
            return response.Animals is List<Animal> animals
                ? Ok(animals)
                : NoContent();
        }

        private async Task<TOut> GetResponseFromRabbitTask<TIn, TOut>(Uri rmqUri, TIn request)
            where TIn : class
            where TOut : class
        {
            var clientFactory = _provider.GetRequiredService<IClientFactory>();

            var client = clientFactory.CreateRequestClient<TIn>(rmqUri);
            var response = await client.GetResponse<TOut>(request);
            return response.Message;
        }
    }
}
