using Microsoft.AspNetCore.Identity;
using MicroZoo.Infrastructure.Models.Roles;
using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Repositories
{
    public interface IUserRolesRepository
    {
        Task<UserWithRoles> GetUserWithRolesAsync_v1(string userId);
        Task<List<Role>> GetRolesOfUserAsync(string userId);
        Task<bool> UpdateUserWithRolesAsync_1(string userId, List<string> RoleIds);
        Task<bool> UpdateUserWithRolesAsync_2(string userId, List<string> RoleIds);
        Task<bool> DeleteUserRolesAsyncPublic(string userId);
        Task<bool> AddUserRolesAsyncPublic(List<IdentityUserRole<string>> userRoles);
    }
}
