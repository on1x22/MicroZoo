using MicroZoo.Infrastructure.Generals;
using MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi;
using MicroZoo.Infrastructure.Models.Jobs.Dto;

namespace MicroZoo.ZookeepersApi.Services
{
    /// <summary>
    /// Interface for processing request from controllers and RabbitMq consumers. If request 
    /// require interconnection with other microservices this actions doing here
    /// </summary>
    public interface IJobsRequestReceivingService
    {
        /// <summary>
        /// Gets all list of jobs of selected zookeeper for all time range
        /// </summary>
        /// <param name="zookeeperId">Zookeeper identifier</param>
        /// <returns></returns>
        Task<GetJobsResponse> GetAllJobsOfZookeeperAsync(int zookeeperId);

        /// <summary>
        /// Gets a list of not finished jobs of selected zookeeper
        /// </summary>
        /// <param name="zookeeperId">Zookeeper identifier</param>
        /// <returns></returns>
        Task<GetJobsResponse> GetCurrentJobsOfZookeeperAsync(int zookeeperId);

        /// <summary>
        /// Gets a list of jobs for the selected zookeeper for the selected time range
        /// </summary>
        /// <param name="zookeeperId">Zookeeper identifier</param>
        /// <param name="dateTimeRange">Selected time range</param>
        /// <param name="orderingOptions">Ordering options for a list of jobs</param>
        /// <param name="pageOptions">Page options for a list of selected jobs</param>
        /// <param name="accessToken">Access token for IdentityApi</param>
        /// <returns></returns>
        Task<GetJobsResponse> GetJobsForDateTimeRangeAsync(int zookeeperId, 
            DateTimeRange dateTimeRange, OrderingOptions orderingOptions, PageOptions pageOptions,
            string accessToken);

        /// <summary>
        /// Adds new job
        /// </summary>
        /// <param name="jobDto">New job data</param>
        /// <param name="accessToken">Access token for IdentityApi</param>
        /// <returns></returns>
        Task<GetJobsResponse> AddJobAsync(JobDto jobDto, string accessToken);

        /// <summary>
        /// Updates data about selected job
        /// </summary>
        /// <param name="jobId">Job identifier</param>
        /// <param name="jobDto">Updated data of job</param>
        /// <param name="accessToken">Access token for IdentityApi</param>
        /// <returns></returns>
        Task<GetJobsResponse> UpdateJobAsync(int jobId, JobWithoutStartTimeDto jobDto, 
                                             string accessToken);

        /// <summary>
        /// Finishes selected job
        /// </summary>
        /// <param name="jobId">Job identifier</param>
        /// <param name="jobReport">Description of results of finished job</param>
        /// <returns></returns>
        Task<GetJobsResponse> FinishJobAsync(int jobId, string jobReport);
    }
}
