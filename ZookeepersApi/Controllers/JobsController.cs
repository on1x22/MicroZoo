using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.Generals;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.ZookeepersApi.Services;
using System.ComponentModel.DataAnnotations;

namespace MicroZoo.ZookeepersApi.Controllers
{
    /// <summary>
    /// Controller for handling jobs requests
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobsRequestReceivingService _receivingService;
        private readonly IResponsesReceiverFromRabbitMq _receiver;
        //private readonly Uri _animalsApiUrl;
        //private readonly Uri _personsApiUrl;
        //private readonly Uri _zookeepersApiUrl;
        private readonly IConnectionService _connectionService;

        public JobsController(IResponsesReceiverFromRabbitMq receiver, 
            IConnectionService connectionService, IJobsRequestReceivingService receivingService)
        {
            _receiver = receiver;
            _connectionService = connectionService;
            _receivingService = receivingService;
        }

        /// <summary>
        /// !!!Use [GET] /Jobs endpoint!!! Get all jobs of specified zookeeper
        /// </summary>
        /// <param name="zookeeperId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="itemsOnPage"></param>
        /// <param name="orderDesc"></param>
        /// <returns>List of jobs</returns>
        [HttpGet("{zookeeperId}")]
        public async Task<IActionResult> GetAllJobsOfZookeeper(int zookeeperId)
        {            
            /*var response = await _receiver.GetResponseFromRabbitTask<GetAllJobsOfZookeeperRequest,
                GetJobsResponse>(new GetAllJobsOfZookeeperRequest(zookeeperId), 
                _connectionService.ZookeepersApiUrl);*/

            var response = await _receivingService.GetAllJobsOfZookeeperAsync(zookeeperId);

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
            /*var response = await _receiver.GetResponseFromRabbitTask<GetCurrentJobsOfZookeeperRequest,
                GetJobsResponse>(new GetCurrentJobsOfZookeeperRequest(zookeeperId),
                _connectionService.ZookeepersApiUrl);*/

            var response = await _receivingService.GetCurrentJobsOfZookeeperAsync(zookeeperId);

            return response.Jobs != null
                ? Ok(response.Jobs)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Get jobs for selected time range
        /// </summary>
        /// <param name="zookeeperId">Id of selected zookeeper</param>
        /// <param name="startDateTime">Date and time from</param>
        /// <param name="finishDateTime">Date and time to</param>
        /// <param name="propertyName">Sorting by column name</param>
        /// <param name="orderDescending">Ordering by descending</param>
        /// <param name="pageNumber">Page number starting from first</param>
        /// <param name="itemsOnPage">Number of records on one page</param>
        /// <returns>List of jobs</returns>
        [HttpGet]
        public async Task<IActionResult> GetJobsForTimeRange(int zookeeperId,
            [FromQuery, Required] DateTime startDateTime, [FromQuery] DateTime finishDateTime,
            [FromQuery] string propertyName, [FromQuery] bool orderDescending,
            [FromQuery] int pageNumber, [FromQuery] int itemsOnPage)
        {
            var dateTimeRange = new DateTimeRange(startDateTime, finishDateTime);
            var orderingOptions = new OrderingOptions(propertyName, orderDescending);
            var pageOptions = new PageOptions(pageNumber, itemsOnPage);

            /*var response = await _receiver.GetResponseFromRabbitTask<GetJobsForDateTimeRangeRequest,
                    GetJobsResponse>(new GetJobsForDateTimeRangeRequest(zookeeperId, 
                    dateTimeRange, orderingOptions, pageOptions), 
                    _connectionService.ZookeepersApiUrl);*/

            var response = await _receivingService.GetJobsForDateTimeRangeAsync(zookeeperId,
                dateTimeRange, orderingOptions, pageOptions);

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
            /*if (jobDto.ZookeeperId <= 0)
                return BadRequest("Zookeeper Id must be more that 0");

            if (jobDto.StartTime != default && jobDto.StartTime < DateTime.UtcNow)
                return BadRequest("Start time less than current time");

            if (jobDto.DeadlineTime == default)
                return BadRequest("Deadline didn't set");

            if (jobDto.DeadlineTime <= jobDto.StartTime)
                return BadRequest("Deadline is less or equal start time");

            if (jobDto.Priority <= 0)
                return BadRequest("Priority must be more than 0");

            if (jobDto.StartTime == default)
                    jobDto.StartTime = DateTime.UtcNow;*/

            /*var response = await _receiver.GetResponseFromRabbitTask<AddJobRequest, GetJobsResponse>(
                new AddJobRequest(jobDto), _connectionService.ZookeepersApiUrl);*/

            var response = await _receivingService.AddJobAsync(jobDto);

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
            /*var response = await _receiver.GetResponseFromRabbitTask<UpdateJobRequest, GetJobsResponse>(
                new UpdateJobRequest(jobId, jobDto), _connectionService.ZookeepersApiUrl);*/

            var response = await _receivingService.UpdateJobAsync(jobId, jobDto);

            return response.Jobs != null
                ? Ok(response.Jobs)
                : BadRequest(response.ErrorMessage);
        }

        /// <summary>
        /// Finish selected jod
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="jobReport"></param>
        /// <returns>List of current jobs</returns>
        [HttpPut("{jobId}/finish")]
        public async Task<IActionResult> FinishJob(int jobId, [FromQuery] string jobReport)
        {
            /*var response = await _receiver.GetResponseFromRabbitTask<FinishJobRequest, GetJobsResponse>(
                new FinishJobRequest(jobId), _connectionService.ZookeepersApiUrl);*/

            var response = await _receivingService.FinishJobAsync(jobId, jobReport);

            return response.Jobs != null
                ? Ok(response.Jobs)
                : BadRequest(response.ErrorMessage);
        }
    }
}
