using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    [Obsolete("Very bad solution. Please use GetJobsForTimeRangeConsumer", true)]
    public class __GetAllJobsForTimeRangeRequest
    {
        public Guid OperationId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime FinishDateTime { get; set; }

        public __GetAllJobsForTimeRangeRequest(DateTime startDateTime, DateTime finishDateTime)
        {
            OperationId = Guid.NewGuid();
            StartDateTime = startDateTime;
            FinishDateTime = finishDateTime;
        }
    }
}
