using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class GetCurrentJobsOfZookeeperRequest
    {
        public Guid OperationId { get; set; }
        public int ZookeeperId { get; set; }

        public GetCurrentJobsOfZookeeperRequest(int zookeeperId)
        {
            OperationId = Guid.NewGuid();
            ZookeeperId = zookeeperId;
        }
    }
}
