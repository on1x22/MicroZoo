using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Services
{
    public interface IRequirementsService
    {
        Task<GetRequirementsResponse> GetAllRequirementsAsync();
        Task<GetRequirementResponse> GetRequirementAsync(Guid requirementId);
        Task<GetRequirementResponse> AddRequirementAsync(RequirementWithoutIdDto requirementDto);
        Task<GetRequirementResponse> DeleteRequirementAsync(Guid requirementId);
    }
}
