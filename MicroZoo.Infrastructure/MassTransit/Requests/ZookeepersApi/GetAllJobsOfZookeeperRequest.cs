
using MicroZoo.Infrastructure.Generals;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class GetAllJobsOfZookeeperRequest
    {
        public Guid OperationId { get; set; }
        public int ZookeeperId { get; set; }
        public PageOptions PageOptions { get; set; }
        public bool OrderDesc { get; set; }

        public GetAllJobsOfZookeeperRequest(int zookeeperId, PageOptions pageOptions, 
            bool orderDesc)
        {
            OperationId = Guid.NewGuid();
            ZookeeperId = zookeeperId;
            PageOptions = pageOptions;
            OrderDesc = orderDesc;
        }
    }
}
