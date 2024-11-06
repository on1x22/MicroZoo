using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.IdentityApi.Services
{
    public interface IUserRolesService
    {
        Task<GetUserWithRolesResponse> GetUserWithRolesAsync(string userId);
    }
}
