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

        public async Task<RoleWithRequirements> GetRoleWithRequirementsAsync(string roleId)
        {
            if (roleId == null)
                return default!;

            var selectedRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            if (selectedRole == null) 
                return default!;

            var requirements = await GetRoleRequirementsAsync(roleId);

            var roleWithRequirements = selectedRole.ConvertToRoleWithRequirements();
            roleWithRequirements.Requirements = requirements;

            return roleWithRequirements;
        }

        private async Task<List<Requirement>> GetRoleRequirementsAsync(string roleId)
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
    }
}
