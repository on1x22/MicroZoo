
using MicroZoo.Infrastructure.Models.Jobs.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class AddJobRequest
    {
        public Guid OperationId { get; set; }
        public JobDto JobDto { get; set; }

        public AddJobRequest(JobDto jobDto)
        {
            OperationId = Guid.NewGuid();
            JobDto = jobDto;
        }
    }
}
