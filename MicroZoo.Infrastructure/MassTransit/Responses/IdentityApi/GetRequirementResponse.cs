using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi
{
    public record GetRequirementResponse
    {
        public Guid OperationId { get; set; }
        public Requirement? Requirement { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
