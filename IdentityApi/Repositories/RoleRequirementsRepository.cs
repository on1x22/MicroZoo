using Microsoft.EntityFrameworkCore;
using MicroZoo.IdentityApi.DbContexts;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Repositories
{
    public class RoleRequirementsRepository : IRoleRequirementsRepository
    {
        private readonly IdentityApiDbContext _dbContext;

        public RoleRequirementsRepository(IdentityApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Requirement>> GetRequirementsOfRoleAsync(string roleId)
        {
            return await _dbContext.Roles.Join(_dbContext.RoleRequirements,
                role => role.Id,
                roleRequirement => roleRequirement.RoleId,
                (role, roleRequirement) => new
                {
                    RoleId = role.Id,
                    RoleRequirementId = roleRequirement.RequirementId
                })
                .Where(r => r.RoleId == roleId)
                .Join(_dbContext.Requirements,
                roleRequirement => roleRequirement.RoleRequirementId,
                requirement => requirement.Id,
                (roleRequirement, requirement) => new Requirement
                {
                    Id = requirement.Id,
                    Name = requirement.Name
                }).ToListAsync();
        }

        public async Task<bool> DeleteRoleRequirementsByRequirementIdAsync(Guid requirementId)
        {
            var roleRequirementsForDelete = await _dbContext.RoleRequirements
                .Where(rr => rr.RequirementId == requirementId).ToListAsync();
            _dbContext.RoleRequirements.RemoveRange(roleRequirementsForDelete);

            return true;
        }
        
        public async Task<bool> DeleteRoleRequirementsByRoleIdAsync(string roleId)
        {
            var roleRequirementsForDelete = await _dbContext.RoleRequirements
                .Where(rr => rr.RoleId == roleId).ToListAsync();
            _dbContext.RoleRequirements.RemoveRange(roleRequirementsForDelete);

            return true;
        }

        public async Task<bool> AddRoleRequirementsAsync(List<RoleRequirement> roleRequirements)
        {
            await _dbContext.RoleRequirements.AddRangeAsync(roleRequirements);
            await SaveChangesAsync();

            return true;
        }

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}
