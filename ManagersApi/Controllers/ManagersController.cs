using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Jobs.Dto;


namespace MicroZoo.ManagersApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly IServiceProvider _provider;
        //private readonly Uri _animalsApiRmqUrl = new Uri("rabbitmq://localhost/animals-queue");
        private readonly Uri _animalsApiUrl;
        private readonly Uri _personsApiUrl;
        private readonly Uri _zookeepersApiUrl;

        public ManagersController(IServiceProvider provider, IConfiguration configuration)
        {
            _provider = provider;
            _animalsApiUrl = new Uri(configuration["ConnectionStrings:AnimalsApiRmq"]);
            _personsApiUrl = new Uri(configuration["ConnectionStrings:PersonsApiRmq"]);
            _zookeepersApiUrl = new Uri(configuration["ConnectionStrings:ZookeepersApiRmq"]);
        }

        [HttpGet("animals")]
        public async Task<IActionResult> GetAllAnimals()
        {
            var response = await GetResponseFromRabbitTask<GetAllAnimalsRequest,
                GetAnimalsResponse>(new GetAllAnimalsRequest(), _animalsApiUrl);
            return response.Animals is List<Animal> animals
                ? Ok(animals)
                : NoContent();
        }

        [HttpPost("jobs")]
        public async Task<IActionResult> AddJob(JobDto jobDto)
        {
            var response = await GetResponseFromRabbitTask<AddJobRequest, GetJobsResponse>(
                new AddJobRequest(jobDto), _zookeepersApiUrl);

            return response.Jobs != null
                ? Ok(response.Jobs)
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
