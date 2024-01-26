
namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class GetJobsForTimeRangeRequest
    {
        public Guid OperationId { get; set; }
        public int ZookeeperId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime FinishDateTime { get; set; }

        public GetJobsForTimeRangeRequest(int zookeeperId, DateTime startDateTime,
            DateTime finishDateTime)
        {
            OperationId = Guid.NewGuid();
            ZookeeperId = zookeeperId;
            StartDateTime = startDateTime;
            FinishDateTime = finishDateTime;
        }
    }
}
