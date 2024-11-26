using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.IdentityApi.Services
{
    public interface IUserRolesService
    {
        Task<GetUserWithRolesResponse> GetUserWithRolesAsync_v1(string userId);
        Task<GetUserWithRolesResponse> GetUserWithRolesAsync_v2(string userId);
        Task<GetUserWithRolesResponse> UpdateUserWithRolesAsync_v1(string userId,
            List<string> RoleIds);
        Task<GetUserWithRolesResponse> UpdateUserWithRolesAsync_v2(string userId,
            List<string> roleIds);
    }
}
