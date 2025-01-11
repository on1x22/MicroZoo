using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi
{
    public record GetRoleResponse
    {
        public Guid OperationId { get; set; }
        public Role? Role { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
