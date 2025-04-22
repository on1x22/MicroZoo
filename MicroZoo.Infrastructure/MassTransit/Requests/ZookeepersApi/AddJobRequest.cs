using MicroZoo.Infrastructure.Models.Jobs.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    /// <summary>
    /// Allows to add job by request receiving from RabbitMq
    /// </summary>
    public class AddJobRequest
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about job
        /// </summary>
        public JobDto JobDto { get; set; }

        /// <summary>
        /// AccessToken for check authentication
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="AddJobRequest"/> class
        /// </summary>
        /// <param name="jobDto"></param>
        /// <param name="accessToken"></param>
        public AddJobRequest(JobDto jobDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            JobDto = jobDto;
            AccessToken = accessToken;
        }
    }
}
