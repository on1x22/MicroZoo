using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MicroZoo.IdentityApi.DbContexts;
using MicroZoo.Infrastructure.Models.Roles;
using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Repositories
{
    public class UserRolesRepository : IUserRolesRepository
    {
        private readonly IdentityApiDbContext _dbContext;

        public UserRolesRepository(IdentityApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserWithRoles> GetUserWithRolesAsync(string userId)
        {
            if (userId == null)
                return default!;

            var selectedUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

            if (selectedUser == null)
                return default!;

            var userRoles = await GetUserRolesAsync(userId);

            var userWithRoles = selectedUser.ConvertToUserWithRoles();
            userWithRoles!.Roles = userRoles;
            return userWithRoles;            
        }
            
        public async Task<bool> UpdateUserWithRolesAsync(string userId, List<string> roleIds)
        {
            if (userId == null)
                return default!;

            var selectedUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (selectedUser == null)
                return default!;

            await DeleteUserRolesAsync(userId);

            var newUserRoles = new List<IdentityUserRole<string>>();
            foreach (var roleId in roleIds)
            {
                var userRole = new IdentityUserRole<string>()
                {
                    UserId = userId,
                    RoleId = roleId
                };
                newUserRoles.Add(userRole);
            }

            await AddUserRolesAsync(newUserRoles);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<List<Role>> GetUserRolesAsync(string userId)
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
                    ConcurrencyStamp = role.ConcurrencyStamp
                }).ToListAsync();
        }

        private async Task AddUserRolesAsync(List<IdentityUserRole<string>> userRoles)
        {
            await _dbContext.UserRoles.AddRangeAsync(userRoles);
        }

        private async Task DeleteUserRolesAsync(string userId)
        {
            var userRolesForDelete = await _dbContext.UserRoles.Where(ur => ur.UserId == userId)
                .ToListAsync();
            _dbContext.UserRoles.RemoveRange(userRolesForDelete);
        }

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();        
    }
}
