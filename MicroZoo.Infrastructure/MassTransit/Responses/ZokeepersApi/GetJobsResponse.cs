using MicroZoo.Infrastructure.Models.Jobs;

namespace MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi
{
    public record GetJobsResponse : IResponseWithError
    {
        public Guid OperationId { get; set; }
        public List<Job> Jobs { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorCodes? ErrorCode { get; set; }
    }
}
