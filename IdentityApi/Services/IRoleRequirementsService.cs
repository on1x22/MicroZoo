using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.IdentityApi.Services
{
    public interface IRoleRequirementsService
    {
        Task<GetRoleWithRequirementsResponse> GetRoleWithRequirementsAsync(string roleId);
        Task<GetRoleWithRequirementsResponse> UpdateRoleWithRequirementsAsync(string roleId,
            List<Guid> requirementIds);
        Task<bool> DeleteRoleRequirementsByRequirementIdAsync(Guid requirementId);
        Task<bool> DeleteRoleRequirementsByRoleIdAsync(string roleId);
    }
}
