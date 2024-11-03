using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.IdentityApi.Services
{
    public interface IUsersRequestReceivingService
    {
        Task<GetUsersResponse> GetAllUsersAsync();
        Task<GetUserResponse> GetUserAsync(string userId);
    }
}
