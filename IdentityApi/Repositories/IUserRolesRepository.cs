using Microsoft.AspNetCore.Identity;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Repositories
{
    public interface IUserRolesRepository
    {
        Task<List<Role>> GetRolesOfUserAsync(string userId);        
        Task<bool> AddUserRolesAsync(List<IdentityUserRole<string>> userRoles);
        Task<bool> DeleteUserRolesByUserIdAsync(string userId);
        Task<bool> DeleteUserRolesByRoleIdAsync(string roleId);
    }
}
