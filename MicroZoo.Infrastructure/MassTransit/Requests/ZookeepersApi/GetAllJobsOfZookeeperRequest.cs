
using MicroZoo.Infrastructure.Generals;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class GetAllJobsOfZookeeperRequest
    {
        public Guid OperationId { get; set; }
        public int ZookeeperId { get; set; }

        public GetAllJobsOfZookeeperRequest(int zookeeperId)
        {
            OperationId = Guid.NewGuid();
            ZookeeperId = zookeeperId;
        }
    }
}
