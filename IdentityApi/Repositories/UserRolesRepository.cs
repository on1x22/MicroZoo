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
    }
}
