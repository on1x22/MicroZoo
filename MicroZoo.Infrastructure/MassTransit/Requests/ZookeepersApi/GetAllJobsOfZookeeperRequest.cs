namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class GetAllJobsOfZookeeperRequest
    {
        public Guid OperationId { get; set; }
        public int ZookeeperId { get; set; }
        public string AccessToken { get; }

        public GetAllJobsOfZookeeperRequest(int zookeeperId, string accessToken)
        {
            OperationId = Guid.NewGuid();
            ZookeeperId = zookeeperId;
            AccessToken = accessToken;
        }
    }
}
