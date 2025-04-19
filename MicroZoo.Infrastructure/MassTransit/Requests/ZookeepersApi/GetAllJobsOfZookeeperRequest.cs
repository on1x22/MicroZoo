namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    /// <summary>
    /// Allows to get list all jobs by request receiving from RabbitMq. This list 
    /// consist of done and undone jobs
    /// </summary>
    public class GetAllJobsOfZookeeperRequest
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
        /// Initialize a new instance of <see cref="GetAllJobsOfZookeeperRequest"/> class
        /// </summary>
        /// <param name="zookeeperId"></param>
        /// <param name="accessToken"></param>
        public GetAllJobsOfZookeeperRequest(int zookeeperId, string accessToken)
        {
            OperationId = Guid.NewGuid();
            ZookeeperId = zookeeperId;
            AccessToken = accessToken;
        }
    }
}
