using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Repositories
{
    public interface IUsersRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserAsync(string userId);        
        //Task<User> CreateUserAsync(User user); 
        Task<User> UpdateUserAsync(string userId, User user);
        Task<User> SoftDeleteUserAsync(User user);
    }
}
