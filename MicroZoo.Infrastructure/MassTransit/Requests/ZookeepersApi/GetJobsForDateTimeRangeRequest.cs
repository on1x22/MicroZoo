
using MicroZoo.Infrastructure.Generals;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class GetJobsForDateTimeRangeRequest
    {
        public Guid OperationId { get; set; }
        public int ZookeeperId { get; set; }
        /*public DateTime StartDateTime { get; set; }
        public DateTime FinishDateTime { get; set; }*/
        public DateTimeRange DateTimeRange { get; set; }
        public OrderingOptions OrderingOptions { get; set; }
        public PageOptions PageOptions { get; set; }

        public GetJobsForDateTimeRangeRequest(int zookeeperId, /*DateTime startDateTime,
            DateTime finishDateTime*/ DateTimeRange dateTimeRange, OrderingOptions orderingOptions,
            PageOptions pageOptions)
        {
            OperationId = Guid.NewGuid();
            ZookeeperId = zookeeperId;
            /*StartDateTime = startDateTime;
            FinishDateTime = finishDateTime;*/
            DateTimeRange = dateTimeRange;
            OrderingOptions = orderingOptions;
            PageOptions = pageOptions;
        }
    }
}
