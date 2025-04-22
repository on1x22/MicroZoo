using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi
{
    /// <summary>
    /// Provides information about list of animals for response through RabbitMq
    /// </summary>
    public record GetAnimalsResponse : IResponseWithError
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about animals
        /// </summary>
        public List<Animal>? Animals { get; set; }

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
