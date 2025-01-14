using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi
{
    public record GetRolesResponse
    {
        public Guid OperationId { get; set; }
        public List<Role>? Roles { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
