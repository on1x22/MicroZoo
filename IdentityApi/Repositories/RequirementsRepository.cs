using Microsoft.EntityFrameworkCore;
using MicroZoo.IdentityApi.DbContexts;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Repositories
{
    public class RequirementsRepository : IRequirementsRepository
    {
        private readonly IdentityApiDbContext _dbContext;

        public RequirementsRepository(IdentityApiDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<List<Requirement>> GetAllRequirementsAsync() =>
            await _dbContext.Requirements.ToListAsync();
        
        public async Task<Requirement> GetRequirementAsync(Guid requirementId) =>   
            await _dbContext.Requirements.FirstOrDefaultAsync(r => r.Id == requirementId);
        
        public async Task<Requirement> AddRequirementAsync(Requirement requirement)
        {
            var duplicatedRequirement = await _dbContext.Requirements.FirstOrDefaultAsync(r =>
                r.Name == requirement.Name);
            if (duplicatedRequirement != null)
                return default;

            _dbContext.Requirements.Add(requirement);
            await _dbContext.SaveChangesAsync();

            return requirement;
        }

        public async Task<Requirement> DeleteRequirementAsync(Guid requirementId)
        {
            var requirement = await _dbContext.Requirements.FirstOrDefaultAsync(r => r.Id == requirementId);
            _dbContext.Requirements.Remove(requirement);
            await _dbContext.SaveChangesAsync();

            return requirement;
        }

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}
