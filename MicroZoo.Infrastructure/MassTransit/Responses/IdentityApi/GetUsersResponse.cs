using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi
{
    public record GetUsersResponse
    {
        public Guid OperationId { get; set; }
        public List<User>? Users { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
