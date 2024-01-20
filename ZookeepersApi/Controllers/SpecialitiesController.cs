using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;

namespace ZookeepersApi.Controllers
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

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok();
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
