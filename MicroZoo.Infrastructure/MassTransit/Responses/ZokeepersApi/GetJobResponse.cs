using MicroZoo.Infrastructure.Models.Jobs;

namespace MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi
{
    /// <summary>
    /// Provides information about job for response through RabbitMq
    /// </summary>
    public class GetJobResponse : IResponseWithError
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about job
        /// </summary>
        public Job? Job { get; set; }

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
