using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Repositories
{
    public interface IRoleRequirementsRepository
    {
        Task<RoleWithRequirements> GetRoleWithRequirementsAsync(string roleId);
        Task<bool> UpdateRoleWithRequirementsAsync(string roleId, List<Guid> requirementIds);
        Task<bool> DeleteRoleRequirementsByRequirementIdAsync(Guid requirementId);
        Task<bool> DeleteRoleRequirementsByRoleIdAsync(string roleId);
    }
}
