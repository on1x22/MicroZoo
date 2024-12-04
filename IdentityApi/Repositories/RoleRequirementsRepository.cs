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

        public async Task<bool> UpdateRoleWithRequirementsAsync(string roleId, 
            List<Guid> requirementIds)
        {
            if(roleId == null)
                return false;

            var selectedRole = _dbContext.Roles.FirstOrDefault(r => r.Id == roleId);

            if (selectedRole == null)
                return false;

            await DeleteRoleRequirementsByRoleIdAsync(roleId);

            var newRoleRequirements = new List<RoleRequirement>();
            foreach (var requirement in requirementIds)
            {
                var roleRequirement = new RoleRequirement()
                {
                    RoleId = roleId,
                    RequirementId = requirement
                };
                newRoleRequirements.Add(roleRequirement);
            }

            await AddRoleRequirementAsync(newRoleRequirements);
            await SaveChangesAsync();

            return true;
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

        private async Task AddRoleRequirementAsync(List<RoleRequirement> roleRequirements) =>
            await _dbContext.RoleRequirements.AddRangeAsync(roleRequirements);

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}
