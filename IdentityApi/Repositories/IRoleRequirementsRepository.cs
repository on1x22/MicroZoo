using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Repositories
{
    public interface IRoleRequirementsRepository
    {
        Task<RoleWithRequirements> GetRoleWithRequirementsAsync(string roleId);
    }
}
