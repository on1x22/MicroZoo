using Microsoft.EntityFrameworkCore;
using MicroZoo.IdentityApi.DbContexts;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly IdentityApiDbContext _dbContext;

        public RolesRepository(IdentityApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Role>> GetAllRolesAsync() =>
             await _dbContext.Roles.ToListAsync();
                
        public async Task<Role> GetRoleAsync(string roleId)
        {
            if (roleId == null)
                return default!;

            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            return role;            
        }

        public async Task<Role> AddRoleAsync(Role role)
        {
            if (role == null)
                return default!;

            await _dbContext.Roles.AddAsync(role);
            await SaveChangesAsync();

            return role;
        }

        public async Task<Role> UpdateRoleAsync(string roleId, Role role)
        {
            if (roleId == null)
                return default!;

            var updatedRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            if (updatedRole == null)
                return default!;

            updatedRole.Update(role);
            await SaveChangesAsync();

            return updatedRole;
        }

        public async Task<Role> DeleteRoleAsync(string roleId)
        {
            if (roleId == null)
                return default!;

            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            _dbContext.Roles.Remove(role);
            await SaveChangesAsync();

            return role;
        }

        public bool CheckEntriesIsExistInDatabase(List<string> roleIds) => 
            roleIds.All(rid => _dbContext.Roles.Any(r => r.Id == rid));
        

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}
