using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi
{
    /// <summary>
    /// Provides information about the user with a list of his roles for response through RabbitMq
    /// </summary>
    public class GetUserWithRolesResponse
    {
        /// <summary>
        /// Id of the request
        /// </summary>
        public Guid OperationId { get; set; }

        /// <summary>
        /// Information about the user with a list of his roles
        /// </summary>
        public UserWithRoles? UserWithRoles { get; set; }

        /// <summary>
        /// Message describing error
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
