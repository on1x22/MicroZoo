using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit.Requests.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.PersonsApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.ZookeepersApi.Models;
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
        /// !!!Use [GET] /Jobs endpoint!!! Get all jobs of specified zookeeper
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

        /// <summary>
        /// Get jobs for selected time range
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="finishDateTime"></param>
        /// <param name="zookeeperId"></param>
        /// <returns>List of jobs</returns>
        [HttpGet]
        public async Task<IActionResult> GetJobsForTimeRange(
            [FromQuery, Required] DateTime startDateTime, [FromQuery] DateTime finishDateTime, 
            int zookeeperId = 0)
        {
            if (zookeeperId < 0)
                return BadRequest("Zookeeper with negative id doesn't exist");

            if (finishDateTime == default)
                finishDateTime = DateTime.UtcNow;

            if (startDateTime >= finishDateTime)
                return BadRequest("Start time more or equals finish time");

            if (zookeeperId > 0)
            {
                var personResponse = await GetResponseFromRabbitTask<GetPersonRequest, GetPersonResponse>(
                    new GetPersonRequest(zookeeperId), _personsApiUrl);

                if (personResponse.Person == null || personResponse.Person.IsManager == true)
                    return BadRequest($"Zookeeper with id={zookeeperId} doesn't exist");
            }

            var response = await GetResponseFromRabbitTask<GetJobsForTimeRangeRequest,
                    GetJobsResponse>(new GetJobsForTimeRangeRequest(zookeeperId, startDateTime,
                    finishDateTime), _zookeepersApiUrl);

            return response.Jobs != null
                    ? Ok(response.Jobs)
                    : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Add new job
        /// </summary>
        /// <param name="jobDto"></param>
        /// <returns>List of current jobs</returns>
        [HttpPost]
        public async Task<IActionResult> AddJob([FromBody] JobDto jobDto)
        {   
            if (jobDto.StartTime != default && jobDto.StartTime < DateTime.UtcNow)
                return BadRequest("Start time less than current time");

            if (jobDto.StartTime == default)
                    jobDto.StartTime = DateTime.UtcNow;

            var jobResponse = await GetResponseFromRabbitTask<AddJobRequest, GetJobResponse>(
                new AddJobRequest(jobDto), _zookeepersApiUrl);

            if (jobResponse.Job == null)
                return BadRequest(jobResponse.ErrorMessage);

            var response = await GetResponseFromRabbitTask<GetCurrentJobsOfZookeeperRequest,
                GetJobsResponse>(new GetCurrentJobsOfZookeeperRequest(jobDto.ZookeeperId), _zookeepersApiUrl);

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
