using MicroZoo.Infrastructure.Models.Jobs.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class UpdateJobRequest
    {
        public Guid OperationId { get; set; }
        public int JobId { get; set; }
        public JobWithoutStartTimeDto JobDto { get; set; }
        public string AccessToken { get; }

        public UpdateJobRequest(int jobId, JobWithoutStartTimeDto jobDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            JobId = jobId;
            JobDto = jobDto;
            AccessToken = accessToken;
        }
    }
}
