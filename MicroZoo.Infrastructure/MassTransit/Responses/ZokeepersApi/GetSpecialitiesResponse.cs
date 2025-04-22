using MicroZoo.Infrastructure.Models.Specialities;

namespace MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi
{
    /// <summary>
    /// Provides information about list of specialities for response through RabbitMq
    /// </summary>
    public record GetSpecialitiesResponse : IResponseWithError
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about list of specialities
        /// </summary>
        public List<Speciality>? Specialities { get; set; }

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
