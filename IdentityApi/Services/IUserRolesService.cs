using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.IdentityApi.Services
{
    public interface IUserRolesService
    {       
        Task<GetUserWithRolesResponse> GetUserWithRolesAsync(string userId);        
        Task<GetUserWithRolesResponse> UpdateUserWithRolesAsync(string userId,
            List<string> roleIds);
        Task<bool> DeleteUserRolesByUserIdAsync(string userId);
        Task<bool> DeleteUserRolesByRoleIdAsync(string roleId);
    }
}
