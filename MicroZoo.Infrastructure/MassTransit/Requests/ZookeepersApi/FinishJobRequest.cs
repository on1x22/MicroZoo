
namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    /// <summary>
    /// Allows to finish job by request receiving from RabbitMq
    /// </summary>
    public class FinishJobRequest
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
        /// Report on the job done
        /// </summary>
        public string JobReport { get; set; }

        /// <summary>
        /// AccessToken for check authentication
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="FinishJobRequest"/> class
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="jobReport"></param>
        /// <param name="accessToken"></param>
        public FinishJobRequest(int jobId, string jobReport, string accessToken)
        {
            OperationId = Guid.NewGuid();
            JobId = jobId;
            JobReport = jobReport;
            AccessToken = accessToken;
        }
    }
}
