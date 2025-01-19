using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi
{
    public class GetRoleWithRequirementsResponse
    {
        public Guid OperationId { get; set; }
        public RoleWithRequirements? RoleWithRequirements { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
