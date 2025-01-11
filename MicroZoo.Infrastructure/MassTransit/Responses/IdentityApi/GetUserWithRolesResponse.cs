using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi
{
    public class GetUserWithRolesResponse
    {
        public Guid OperationId { get; set; }
        public UserWithRoles UserWithRoles { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
