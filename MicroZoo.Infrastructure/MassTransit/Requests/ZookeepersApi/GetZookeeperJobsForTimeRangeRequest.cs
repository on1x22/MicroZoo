
namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class GetZookeeperJobsForTimeRangeRequest
    {
        public Guid OperationId { get; set; }
        public int ZookeeperId { get; set; }
        DateTime StartDateTime { get; set; }
        DateTime FinishDateTime { get; set; }

        public GetZookeeperJobsForTimeRangeRequest(int zookeeperId, DateTime startDateTime,
            DateTime finishDateTime)
        {
            OperationId = Guid.NewGuid();
            ZookeeperId = zookeeperId;
            StartDateTime = startDateTime;
            FinishDateTime = finishDateTime;
        }
    }
}
