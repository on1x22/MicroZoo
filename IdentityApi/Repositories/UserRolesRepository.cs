using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroZoo.IdentityApi.DbContexts;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Repositories
{
    public class UserRolesRepository : IUserRolesRepository
    {
        private readonly IdentityApiDbContext _dbContext;

        public UserRolesRepository(IdentityApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Role>> GetRolesOfUserAsync(string userId)
        {
            return await _dbContext.Users.Join(_dbContext.UserRoles,
                user => user.Id,
                userRoles => userRoles.UserId,
                (user, userRoles) => new
                {
                    UserId = user.Id,
                    RoleId = userRoles.RoleId
                })
                .Where(u => u.UserId == userId)
                .Join(_dbContext.Roles,
                userRoles => userRoles.RoleId,
                role => role.Id,
                (userRoles, role) => new Role
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    NormalizedName = role.NormalizedName,
                    ConcurrencyStamp = role.ConcurrencyStamp,
                    Deleted = role.Deleted
                }).ToListAsync();
        }
        public async Task<bool> AddUserRolesAsync(List<IdentityUserRole<string>> userRoles)
        {
            await _dbContext.UserRoles.AddRangeAsync(userRoles);
            await SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUserRolesByUserIdAsync(string userId)
        {
            var userRolesForDelete = await _dbContext.UserRoles.Where(ur => ur.UserId == userId)
                .ToListAsync();
            _dbContext.UserRoles.RemoveRange(userRolesForDelete);
            return true;
        }

        public async Task<bool> DeleteUserRolesByRoleIdAsync(string roleId)
        {
            var userRolesForDelete = await _dbContext.UserRoles.Where(ur => ur.RoleId == roleId)
                .ToListAsync();
            _dbContext.UserRoles.RemoveRange(userRolesForDelete);
            return true;
        }

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();        
    }
}
