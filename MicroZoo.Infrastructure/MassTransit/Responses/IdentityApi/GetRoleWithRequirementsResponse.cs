using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi
{
    /// <summary>
    /// Provides information about role with requirements for response through RabbitMq
    /// </summary>
    public class GetRoleWithRequirementsResponse
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about role with requirements
        /// </summary>
        public RoleWithRequirements? RoleWithRequirements { get; set; }

        /// <summary>
        /// Message describing error
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
