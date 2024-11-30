using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Repositories
{
    public interface IRolesRepository
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleAsync(string roleId);
        Task<Role> AddRoleAsync(Role role);
        Task<Role> UpdateRoleAsync(string roleId, Role role);
        Task<Role> DeleteRoleAsync(string roleId);
        bool CheckEntriesIsExistInDatabase(List<string> roleIds);
    }
}
