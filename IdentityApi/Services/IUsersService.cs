using MicroZoo.IdentityApi.Models.DTO;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.IdentityApi.Services
{
    public interface IUsersService
    {
        Task<GetUsersResponse> GetAllUsersAsync();
        Task<GetUserResponse> GetUserAsync(string userId);
        Task<GetUserResponse> UpdateUserAsync(string userId, UserForUpdateDto userForUpdateDto);
        Task<GetUserResponse> SoftDeleteUserAsync(string userId);
    }
}
