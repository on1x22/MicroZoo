using MicroZoo.IdentityApi.DbContexts;
using MicroZoo.Infrastructure.Models.Users;
using Microsoft.EntityFrameworkCore;

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
            /*if (userId == null)
                return default;*/

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId && 
                                                                  u.Deleted == false);
            //var user = await GetUserFromDbAsync(userId);
            return user!;
        }

        /*public async Task<User> CreateUserAsync(User user)
        {
            if (user == null)
                return default;

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }*/
         
        public async Task<User> UpdateUserAsync(string userId, User user)
        {
            /*if (userId == null)
                return default;*/

            //var updatedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            /*var updatedUser = await GetUserAsync(userId); //GetUserFromDbAsync(userId);
            if (updatedUser == null)
                return default;*/

            //updatedUser.Update(user);
            user.Id = userId;
            _dbContext.Update(user);

            await SaveChangesAsync();
            return /*updatedUser*/user;
        }

        public async Task<User> SoftDeleteUserAsync(User user)
        {
            /*if (userId == null)
                return default;*/

            //var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            //var user = await GetUserAsync(userId);
            //_dbContext.Users.Remove(user);
            //await _dbContext.Users.Where(u => u.Id == userId).ExecuteDeleteAsync();
            user.Deleted = true;
            _dbContext.Update(user);

            await SaveChangesAsync();

            return user;
        }

        /*private async Task<User?> GetUserFromDbAsync(string userId) =>
            await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);*/

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}
