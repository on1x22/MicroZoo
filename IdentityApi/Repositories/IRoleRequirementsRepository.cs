using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Repositories
{
    public interface IRoleRequirementsRepository
    {
        Task<List<Requirement>> GetRequirementsOfRoleAsync(string roleId);
        Task<bool> AddRoleRequirementsAsync(List<RoleRequirement> roleRequirements);
        Task<bool> DeleteRoleRequirementsByRequirementIdAsync(Guid requirementId);
        Task<bool> DeleteRoleRequirementsByRoleIdAsync(string roleId);
    }
}
