using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi
{
    /// <summary>
    /// Provides information about requirement for response through RabbitMq
    /// </summary>
    public record GetRequirementResponse
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about requirement
        /// </summary>
        public Requirement? Requirement { get; set; }

        /// <summary>
        /// Message describing error
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
