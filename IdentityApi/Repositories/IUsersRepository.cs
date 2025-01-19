using MicroZoo.IdentityApi.Models.DTO;
using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Repositories
{
    public interface IUsersRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserAsync(string userId);    
        Task<User> UpdateUserAsync(string userId, UserForUpdateDto userForUpdateDto);
        Task<User> SoftDeleteUserAsync(User user);
    }
}
