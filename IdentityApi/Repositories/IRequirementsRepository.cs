using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Repositories
{
    public interface IRequirementsRepository
    {
        Task<List<Requirement>> GetAllRequirementsAsync();
        Task<Requirement> GetRequirementAsync(Guid requirementId);
        Task<Requirement> AddRequirementAsync(Requirement requirement);
        Task<Requirement> SoftDeleteRequirementAsync(Requirement requirement);
    }
}
