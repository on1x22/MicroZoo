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
             await _dbContext.Roles.Where(r => r.Deleted == false).ToListAsync();
                
        public async Task<Role> GetRoleAsync(string roleId)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId &&
                                                                  r.Deleted == false);
            return role!;            
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

            var updatedRole = await GetRoleAsync(roleId);
            if (updatedRole == null)
                return default!;

            updatedRole.Update(role);
            await SaveChangesAsync();

            return updatedRole;
        }

        public async Task<Role> SoftDeleteRoleAsync(Role role)
        {
            role.Deleted = true;
            _dbContext.Update(role);

            await SaveChangesAsync();

            return role;
        }

        public bool CheckEntriesIsExistInDatabase(List<string> roleIds) => 
            roleIds.All(rid => _dbContext.Roles.Any(r => r.Id == rid && r.Deleted == false));
       
        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}
