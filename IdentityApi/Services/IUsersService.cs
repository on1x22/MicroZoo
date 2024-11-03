using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.IdentityApi.Services
{
    public interface IUsersService
    {
        Task<GetUsersResponse> GetAllUsersAsync();
        Task<GetUserResponse> GetUserAsync(string userId);
    }
}
