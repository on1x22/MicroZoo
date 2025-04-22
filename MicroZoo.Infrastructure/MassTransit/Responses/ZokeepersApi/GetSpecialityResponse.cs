using MicroZoo.Infrastructure.Models.Specialities;

namespace MicroZoo.Infrastructure.MassTransit.Responses.ZokeepersApi
{
    /// <summary>
    /// Provides information about speciality for response through RabbitMq
    /// </summary>
    public record GetSpecialityResponse : IResponseWithError
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about speciality
        /// </summary>
        public Speciality? Speciality { get; set; }

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
