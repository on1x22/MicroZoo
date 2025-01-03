using ManagersApi.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.AuthService.Services;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Animals;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.JwtConfiguration;


namespace MicroZoo.ManagersApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly IServiceProvider _provider;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConnectionService _connectionService;
        private readonly Uri _animalsApiUrl;
        private readonly Uri _personsApiUrl;
        private readonly Uri _zookeepersApiUrl;

        public ManagersController(IServiceProvider provider, IConfiguration configuration,
            IAuthorizationService authorizationService,
            IConnectionService connectionService)
        {
            _provider = provider;
            _authorizationService = authorizationService;
            _connectionService = connectionService;
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
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(Request);
            var accessResult = await _authorizationService.CheckAccessInIdentityApiAsync(
                accessToken: accessToken,
                type: typeof(ManagersController),
                methodName: nameof(AddJob),
                identityApiUrl: _connectionService.IdentityApi);

            if (!accessResult.IsAccessAllowed)
                return accessResult.Result;

            var response = await GetResponseFromRabbitTask<AddJobRequest, GetJobsResponse>(
                new AddJobRequest(jobDto, accessToken), _zookeepersApiUrl);

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
