using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi
{
    /// <summary>
    /// Provides information about list of requirements for response through RabbitMq
    /// </summary>
    public record GetRequirementsResponse
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about list of requirement
        /// </summary>
        public List<Requirement>? Requirements { get; set; }

        /// <summary>
        /// Message describing error
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
