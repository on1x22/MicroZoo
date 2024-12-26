﻿using Microsoft.AspNetCore.Mvc;
using MicroZoo.Infrastructure.Generals;
using MicroZoo.Infrastructure.MassTransit;
using MicroZoo.Infrastructure.Models.Jobs.Dto;
using MicroZoo.JwtConfiguration;
using MicroZoo.ZookeepersApi.Policies;
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
        private readonly IAuthorizationService _authorizationService;
        /*private readonly IResponsesReceiverFromRabbitMq _receiver;*/
        private readonly IConnectionService _connectionService;

        /// <summary>
        /// Controller for handling jobs requests
        /// </summary>
        public JobsController(/*IResponsesReceiverFromRabbitMq receiver,*/ 
            IConnectionService connectionService, IJobsRequestReceivingService receivingService,
            IAuthorizationService authorizationService)
        {
            /*_receiver = receiver;*/
            _connectionService = connectionService;
            _receivingService = receivingService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// !!!Use [GET] /Jobs endpoint!!! Get all jobs of specified zookeeper
        /// </summary>
        /// <param name="zookeeperId"></param>
        /// <returns>List of jobs</returns>
        [Obsolete]
        [HttpGet("{zookeeperId}")]
        public async Task<IActionResult> GetAllJobsOfZookeeper(int zookeeperId)
        {       
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
        [PolicyValidation(Policy = "ZookeepersApi.Read")]        
        public async Task<IActionResult> GetCurrentJobsOfZookeeper(int zookeeperId)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(HttpContext.Request);
            var methodName = nameof(GetCurrentJobsOfZookeeper);
            var endpointPolicies = PoliciesValidator.GetPolicies(typeof(JobsController), methodName);
            
            if (accessToken == null || (endpointPolicies == null || endpointPolicies.Count == 0))
                return Unauthorized();

            var accessResponse = await _authorizationService.IsResourceAccessConfirmed(accessToken,
                endpointPolicies);
            if (accessResponse.ErrorMessage != null)
                return BadRequest(accessResponse.ErrorMessage);

            if (!accessResponse.IsAuthenticated)
                return Unauthorized();

            if (!accessResponse.IsAccessConfirmed)
                return Forbid();

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
        [PolicyValidation(Policy = "ZookeepersApi.Read")]
        public async Task<IActionResult> GetJobsForTimeRange(
            [FromQuery, Required] DateTime startDateTime, [FromQuery] DateTime finishDateTime,
            int zookeeperId = 0,
            [FromQuery] string propertyName = "DeadlineTime", [FromQuery] bool orderDescending = false,
            [FromQuery] int pageNumber = 1, [FromQuery] int itemsOnPage = 20)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(HttpContext.Request);
            var methodName = nameof(GetJobsForTimeRange);
            var endpointPolicies = PoliciesValidator.GetPolicies(typeof(JobsController), methodName);

            if (accessToken == null || (endpointPolicies == null || endpointPolicies.Count == 0))
                return Unauthorized();

            var accessResponse = await _authorizationService.IsResourceAccessConfirmed(accessToken,
                endpointPolicies);
            if (accessResponse.ErrorMessage != null)
                return BadRequest(accessResponse.ErrorMessage);

            if (!accessResponse.IsAuthenticated)
                return Unauthorized();

            if (!accessResponse.IsAccessConfirmed)
                return Forbid();

            var dateTimeRange = new DateTimeRange(startDateTime, finishDateTime);
            var orderingOptions = new OrderingOptions(propertyName, orderDescending);
            var pageOptions = new PageOptions(pageNumber, itemsOnPage);

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
        [PolicyValidation(Policy = "ZookeepersApi.Create")]
        public async Task<IActionResult> AddJob([FromBody] JobDto jobDto)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(HttpContext.Request);
            var methodName = nameof(AddJob);
            var endpointPolicies = PoliciesValidator.GetPolicies(typeof(JobsController), methodName);

            if (accessToken == null || (endpointPolicies == null || endpointPolicies.Count == 0))
                return Unauthorized();

            var accessResponse = await _authorizationService.IsResourceAccessConfirmed(accessToken,
                endpointPolicies);
            if (accessResponse.ErrorMessage != null)
                return BadRequest(accessResponse.ErrorMessage);

            if (!accessResponse.IsAuthenticated)
                return Unauthorized();

            if (!accessResponse.IsAccessConfirmed)
                return Forbid();

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
        [PolicyValidation(Policy = "ZookeepersApi.Update")]
        public async Task<IActionResult> UpdateJob(int jobId, [FromBody] JobWithoutStartTimeDto jobDto)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(HttpContext.Request);
            var methodName = nameof(UpdateJob);
            var endpointPolicies = PoliciesValidator.GetPolicies(typeof(JobsController), methodName);

            if (accessToken == null || (endpointPolicies == null || endpointPolicies.Count == 0))
                return Unauthorized();

            var accessResponse = await _authorizationService.IsResourceAccessConfirmed(accessToken,
                endpointPolicies);
            if (accessResponse.ErrorMessage != null)
                return BadRequest(accessResponse.ErrorMessage);

            if (!accessResponse.IsAuthenticated)
                return Unauthorized();

            if (!accessResponse.IsAccessConfirmed)
                return Forbid();

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
        [PolicyValidation(Policy = "ZookeepersApi.Update")]
        public async Task<IActionResult> FinishJob(int jobId, [FromQuery] string jobReport)
        {
            var accessToken = JwtExtensions.GetAccessTokenFromRequest(HttpContext.Request);
            var methodName = nameof(FinishJob);
            var endpointPolicies = PoliciesValidator.GetPolicies(typeof(JobsController), methodName);

            if (accessToken == null || (endpointPolicies == null || endpointPolicies.Count == 0))
                return Unauthorized();

            var accessResponse = await _authorizationService.IsResourceAccessConfirmed(accessToken,
                endpointPolicies);
            if (accessResponse.ErrorMessage != null)
                return BadRequest(accessResponse.ErrorMessage);

            if (!accessResponse.IsAuthenticated)
                return Unauthorized();

            if (!accessResponse.IsAccessConfirmed)
                return Forbid();

            var response = await _receivingService.FinishJobAsync(jobId, jobReport);

            return response.Jobs != null
                ? Ok(response.Jobs)
                : BadRequest(response.ErrorMessage);
        }
    }
}
