using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ZookeepersApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ZookeepersController : ControllerBase
    {
        private readonly IServiceProvider _provider;
        private readonly Uri _animalsApiUrl;
        private readonly Uri _personsApiUrl;

        public ZookeepersController(IServiceProvider provider, IConfiguration configuration)
        {
            _provider = provider;
            _animalsApiUrl = new Uri(configuration["ConnectionStrings:AnimalsApiRmq"]);
            _personsApiUrl = new Uri(configuration["ConnectionStrings:PersonsApiRmq"]);
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok();
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
