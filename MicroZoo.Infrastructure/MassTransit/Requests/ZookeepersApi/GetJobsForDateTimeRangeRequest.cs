
using MicroZoo.Infrastructure.Generals;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    /// <summary>
    /// Allows to get list of jobs in selected date and time range 
    /// by request receiving from RabbitMq
    /// </summary>
    public class GetJobsForDateTimeRangeRequest
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
        /// The time range between which the jobs of the selected zookeeper are searched
        /// </summary>
        public DateTimeRange DateTimeRange { get; set; }

        /// <summary>
        /// Options that allows to order searched jobs
        /// </summary>
        public OrderingOptions OrderingOptions { get; set; }

        /// <summary>
        /// Options that allows to select quantity and page number of searched jobs
        /// </summary>
        public PageOptions PageOptions { get; set; }

        /// <summary>
        /// AccessToken for check authentication
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="GetJobsForDateTimeRangeRequest"/> class
        /// </summary>
        /// <param name="zookeeperId"></param>
        /// <param name="dateTimeRange"></param>
        /// <param name="orderingOptions"></param>
        /// <param name="pageOptions"></param>
        /// <param name="accessToken"></param>
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
