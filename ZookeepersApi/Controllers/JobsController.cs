using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using System.ComponentModel.DataAnnotations;

namespace MicroZoo.ZookeepersApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IServiceProvider _provider;
        private readonly Uri _animalsApiUrl;
        private readonly Uri _personsApiUrl;
        private readonly Uri _zookeepersApiUrl;

        public JobsController(IServiceProvider provider, IConfiguration configuration)
        {
            _provider = provider;
            _animalsApiUrl = new Uri(configuration["ConnectionStrings:AnimalsApiRmq"]);
            _personsApiUrl = new Uri(configuration["ConnectionStrings:PersonsApiRmq"]);
            _zookeepersApiUrl = new Uri(configuration["ConnectionStrings:ZookeepersApiRmq"]);
        }

        /// <summary>
        /// Get all jobs of specified zookeeper
        /// </summary>
        /// <param name="zookeeperId"></param>
        /// <returns>List of jobs</returns>
        [HttpGet("{zookeeperId}")]
        public async Task<IActionResult> GetAllJobsOfZookeeper(int zookeeperId)
        {
            var response = await GetResponseFromRabbitTask<GetAllJobsOfZookeeperRequest,
                GetJobsResponse>(new GetAllJobsOfZookeeperRequest(zookeeperId), _zookeepersApiUrl);

            return response.Jobs != null
                ? Ok(response.Jobs)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Get unfinished jobs of specified zookeeper
        /// </summary>
        /// <param name="zookeeperId"></param>
        /// <returns>List of jobs</returns>
        [HttpGet("{zookeeperId}/current")]
        public async Task<IActionResult> GetCurrentJobsOfZookeeper(int zookeeperId)
        {
            var response = await GetResponseFromRabbitTask<GetCurrentJobsOfZookeeperRequest,
                GetJobsResponse>(new GetCurrentJobsOfZookeeperRequest(zookeeperId), _zookeepersApiUrl);

            return response.Jobs != null
                ? Ok(response.Jobs)
                : BadRequest(response.ErrorMessage);
        }

        [HttpGet("{zookeeperId}/range")]
        public async Task<IActionResult> GetZookeeperJobsForTimeRange(int zookeeperId,
            [FromQuery, Required] DateTime startDateTime, [FromQuery] DateTime finishDateTime)
        {
            if (startDateTime >= finishDateTime)
                return BadRequest("Start time more or equals finish time");
            
            var response = await GetResponseFromRabbitTask<GetZookeeperJobsForTimeRangeRequest,
                GetJobsResponse>(new GetZookeeperJobsForTimeRangeRequest(zookeeperId, startDateTime, 
                finishDateTime), _zookeepersApiUrl);

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
