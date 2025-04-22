namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    /// <summary>
    /// Allows to get list of started but undoned jobs by request receiving from RabbitMq
    /// </summary>
    public class GetCurrentJobsOfZookeeperRequest
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Zookeeper's Id
        /// </summary>
        public int ZookeeperId { get; set; }

        /// <summary>
        /// AccessToken for check authentication
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="GetCurrentJobsOfZookeeperRequest"/> class
        /// </summary>
        /// <param name="zookeeperId"></param>
        /// <param name="accessToken"></param>
        public GetCurrentJobsOfZookeeperRequest(int zookeeperId, string accessToken)
        {
            OperationId = Guid.NewGuid();
            ZookeeperId = zookeeperId;
            AccessToken = accessToken;
        }
    }
}
