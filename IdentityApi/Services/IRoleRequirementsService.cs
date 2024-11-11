using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Services
{
    public interface IRoleRequirementsService
    {
        Task<GetRoleWithRequirementsResponse> GetRoleWithRequirementsAsync(string roleId);
        Task<GetRoleWithRequirementsResponse> UpdateRoleWithRequirementsAsync(string roleId,
            List<Guid> requirementIds);
    }
}
