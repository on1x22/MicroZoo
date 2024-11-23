﻿using MicroZoo.IdentityApi.DbContexts;
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
            await _dbContext.Users.ToListAsync();

        public async Task<User> GetUserAsync(string userId)
        {
            if (userId == null)
                return default;

            //var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var user = await GetUserFromDbAsync(userId);
            return user;
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
            if (userId == null)
                return default;

            //var updatedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var updatedUser = await GetUserFromDbAsync(userId);
            if (updatedUser == null)
                return default;

            updatedUser.Update(user);

            await SaveChangesAsync();
            return updatedUser;
        }

        public async Task<User> DeleteUserAsync(string userId)
        {
            if (userId == null)
                return default;

            //var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var user = await GetUserFromDbAsync(userId);
            _dbContext.Users.Remove(user);
            await SaveChangesAsync();

            return user;
        }

        private async Task<User?> GetUserFromDbAsync(string userId) =>
            await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        private async Task SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();
    }
}
