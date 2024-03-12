
namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class GetJobsForDateTimeRangeRequest
    {
        public Guid OperationId { get; set; }
        public int ZookeeperId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime FinishDateTime { get; set; }

        public GetJobsForDateTimeRangeRequest(int zookeeperId, DateTime startDateTime,
            DateTime finishDateTime)
        {
            OperationId = Guid.NewGuid();
            ZookeeperId = zookeeperId;
            StartDateTime = startDateTime;
            FinishDateTime = finishDateTime;
        }
    }
}
