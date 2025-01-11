using MicroZoo.IdentityApi.DbContexts;
using MicroZoo.Infrastructure.Models.Users;
using Microsoft.EntityFrameworkCore;
using MicroZoo.IdentityApi.Models.DTO;
using MicroZoo.IdentityApi.Models.Mappers;

namespace MicroZoo.IdentityApi.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IdentityApiDbContext _dbContext;

        public UsersRepository(IdentityApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetAllUsersAsync() =>
            await _dbContext.Users.Where(u => u.Deleted == false).ToListAsync();

        public async Task<User> GetUserAsync(string userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId && 
                                                                  u.Deleted == false);
            
            return user!;
        }

        public async Task<User> UpdateUserAsync(string userId, UserForUpdateDto userForUpdateDto)
        {
            var userForUpdate = await GetUserAsync(userId);

            if (userForUpdate == null)  
                return default!;
                 
            userForUpdate = UserUpdater.UpdateFromUserForUpdateDto(userForUpdateDto, userForUpdate);
           
            await SaveChangesAsync();

            return userForUpdate;
        }

        public async Task<User> SoftDeleteUserAsync(User user)
        {            
            user.Deleted = true;
            _dbContext.Update(user);

            await SaveChangesAsync();

            return user;
        }

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}
