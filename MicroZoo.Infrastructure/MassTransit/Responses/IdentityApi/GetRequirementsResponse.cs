using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi
{
    public record GetRequirementsResponse
    {
        public Guid OperationId { get; set; }
        public List<Requirement>? Requirements { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
