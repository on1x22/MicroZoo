using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.Infrastructure.MassTransit.Responses.AnimalsApi
{
    /// <summary>
    /// Provides information about animal for response through RabbitMq
    /// </summary>
    public record GetAnimalResponse : IResponseWithError
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about animal
        /// </summary>
        public Animal? Animal { get; set; }

        /// <summary>
        /// Message describing erro
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Http code of occurred error
        /// </summary>
        public ErrorCodes? ErrorCode { get; set; }
    }
}
