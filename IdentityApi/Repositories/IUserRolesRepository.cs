using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Repositories
{
    public interface IUserRolesRepository
    {
        Task<UserWithRoles> GetUserWithRolesAsync(string userId);        
        Task<bool> UpdateUserWithRolesAsync(string userId, List<string> RoleIds);
    }
}
