using MicroZoo.Infrastructure.Models.Jobs.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class UpdateJobRequest
    {
        public Guid OperationId { get; set; }
        public int JobId { get; set; }
        public JobWithoutStartTimeDto JobDto { get; set; }

        public UpdateJobRequest(int jobId, JobWithoutStartTimeDto jobDto)
        {
            OperationId = Guid.NewGuid();
            JobId = jobId;
            JobDto = jobDto;
        }
    }
}
