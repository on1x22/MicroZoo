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
            await _dbContext.Requirements.Where(r => r.Deleted == false).ToListAsync();

        public async Task<Requirement> GetRequirementAsync(Guid requirementId)
        {
            var requirement = await _dbContext.Requirements.FirstOrDefaultAsync(
                                                                r => r.Id == requirementId &&
                                                                r.Deleted == false);

            return requirement!;
        }
        
        public async Task<Requirement> AddRequirementAsync(Requirement requirement)
        {
            var duplicatedRequirement = await _dbContext.Requirements.FirstOrDefaultAsync(r =>
                r.Name == requirement.Name);
            if (duplicatedRequirement != null)
                return default!;

            _dbContext.Requirements.Add(requirement);
            await SaveChangesAsync();

            return requirement;
        }

        public async Task<Requirement> SoftDeleteRequirementAsync(Requirement requirement)
        {            
            requirement.Deleted = true;
            _dbContext.Update(requirement);

            await SaveChangesAsync();

            return requirement;
        }

        public bool CheckEntriesAreExistInDatabase(List<Guid> requirementIds) =>
            requirementIds.All(reqId => _dbContext.Requirements.Any(req => req.Id == reqId &&
                                                                    req.Deleted == false));

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}
