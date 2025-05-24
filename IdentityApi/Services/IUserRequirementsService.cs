using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Services
{
    public interface IUserRequirementsService
    {
        Task<List<string>> GetAllowedRequirementsOfUser(User user);
    }
}
