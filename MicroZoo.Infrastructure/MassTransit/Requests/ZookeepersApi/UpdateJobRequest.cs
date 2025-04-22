using MicroZoo.Infrastructure.Models.Jobs.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    /// <summary>
    /// Allows to update information about job by request receiving from RabbitMq
    /// </summary>
    public class UpdateJobRequest
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Job's Id
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// New information about the job that will be updated
        /// </summary>
        public JobWithoutStartTimeDto JobDto { get; set; }

        /// <summary>
        /// AccessToken for check authentication
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="UpdateJobRequest"/> class
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="jobDto"></param>
        /// <param name="accessToken"></param>
        public UpdateJobRequest(int jobId, JobWithoutStartTimeDto jobDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            JobId = jobId;
            JobDto = jobDto;
            AccessToken = accessToken;
        }
    }
}
