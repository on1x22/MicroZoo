using MicroZoo.Infrastructure.Models.Jobs;

namespace MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi
{
    /// <summary>
    /// Provides information about list of jobs for response through RabbitMq
    /// </summary>
    public record GetJobsResponse : IResponseWithError
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about list of jobs
        /// </summary>
        public List<Job>? Jobs { get; set; }

        /// <summary>
        /// Message describing error
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Http code of occurred error
        /// </summary>
        public ErrorCodes? ErrorCode { get; set; }
    }
}
