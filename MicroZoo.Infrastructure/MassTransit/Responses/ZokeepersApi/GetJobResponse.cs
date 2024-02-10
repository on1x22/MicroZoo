using MicroZoo.Infrastructure.Models.Jobs;

namespace MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi
{
    public class GetJobResponse
    {
        public Guid OperationId { get; set; }
        public Job Job { get; set; }
        public string ErrorMessage { get; set; }
    }
}
