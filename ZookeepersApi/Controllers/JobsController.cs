using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.MassTransit;
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
        private readonly IResponsesReceiverFromRabbitMq _receiver;
        private readonly Uri _animalsApiUrl;
        private readonly Uri _personsApiUrl;
        private readonly Uri _zookeepersApiUrl;

        public JobsController(IServiceProvider provider, IResponsesReceiverFromRabbitMq receiver,
            IConfiguration configuration)
        {
            _provider = provider;
            _receiver = receiver;
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
            var response = await _receiver.GetResponseFromRabbitTask<GetAllJobsOfZookeeperRequest,
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
            var response = await _receiver.GetResponseFromRabbitTask<GetCurrentJobsOfZookeeperRequest,
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
            var response = await _receiver.GetResponseFromRabbitTask<GetJobsForTimeRangeRequest,
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

            var response = await _receiver.GetResponseFromRabbitTask<AddJobRequest, GetJobsResponse>(
                new AddJobRequest(jobDto), _zookeepersApiUrl);

            return response.Jobs != null
                ? Ok(response.Jobs)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Update task info
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="jobDto"></param>
        /// <returns>List of current jobs</returns>
        [HttpPut("{jobId}")]
        public async Task<IActionResult> UpdateJob(int jobId, [FromBody] JobWithoutStartTimeDto jobDto)
        {
            var response = await _receiver.GetResponseFromRabbitTask<UpdateJobRequest, GetJobsResponse>(
                new UpdateJobRequest(jobId, jobDto), _zookeepersApiUrl);

            return response.Jobs != null
                ? Ok(response.Jobs)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Finish selected jod
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns>List of current jobs</returns>
        [HttpPut("{jobId}/finish")]
        public async Task<IActionResult> FinishJob(int jobId)
        {
            var response = await _receiver.GetResponseFromRabbitTask<FinishJobRequest, GetJobsResponse>(
                new FinishJobRequest(jobId), _zookeepersApiUrl);

            return response.Jobs != null
                ? Ok(response.Jobs)
                : BadRequest(response.ErrorMessage);
        }

        /*[Obsolete("Use _receiver.GetResponseFromRabbitTask()")]
        private async Task<TOut> GetResponseFromRabbitTask<TIn, TOut>(TIn request, Uri rabbitMqUrl)
            where TIn : class
            where TOut : class
        {
            var clientFactory = _provider.GetRequiredService<IClientFactory>();

            var client = clientFactory.CreateRequestClient<TIn>(rabbitMqUrl);
            var response = await client.GetResponse<TOut>(request);
            return response.Message;
        }*/
    }
}
