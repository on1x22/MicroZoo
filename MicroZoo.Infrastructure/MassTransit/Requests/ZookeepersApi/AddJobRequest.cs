
using MicroZoo.Infrastructure.Models.Jobs.Dto;

namespace MicroZoo.Infrastructure.MassTransit.Requests.ZookeepersApi
{
    public class AddJobRequest
    {
        public Guid OperationId { get; set; }
        public JobDto JobDto { get; set; }
        public string AccessToken { get; }

        public AddJobRequest(JobDto jobDto, string accessToken)
        {
            OperationId = Guid.NewGuid();
            JobDto = jobDto;
            AccessToken = accessToken;
        }
    }
}
