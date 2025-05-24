using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Repositories
{
    public interface IUserRequirementsRepository
    {
        Task<List<string>> GetAllowedRequirementsOfUser(User user);
    }
}
