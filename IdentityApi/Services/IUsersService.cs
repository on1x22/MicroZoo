using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Services
{
    public interface IUsersService
    {
        Task<GetUsersResponse> GetAllUsersAsync();
        Task<GetUserResponse> GetUserAsync(string userId);
        Task<GetUserResponse> UpdateUserAsync(string userId, User user);
    }
}
