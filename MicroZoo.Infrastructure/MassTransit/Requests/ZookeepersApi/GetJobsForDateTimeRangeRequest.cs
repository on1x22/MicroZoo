
using MicroZoo.Infrastructure.Generals;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class GetJobsForDateTimeRangeRequest
    {
        public Guid OperationId { get; set; }
        public int ZookeeperId { get; set; }
        public DateTimeRange DateTimeRange { get; set; }
        public OrderingOptions OrderingOptions { get; set; }
        public PageOptions PageOptions { get; set; }
        public string AccessToken { get; }

        public GetJobsForDateTimeRangeRequest(int zookeeperId, DateTimeRange dateTimeRange, 
            OrderingOptions orderingOptions, PageOptions pageOptions, string accessToken)
        {
            OperationId = Guid.NewGuid();
            ZookeeperId = zookeeperId;
            DateTimeRange = dateTimeRange;
            OrderingOptions = orderingOptions;
            PageOptions = pageOptions;
            AccessToken = accessToken;
        }
    }
}
