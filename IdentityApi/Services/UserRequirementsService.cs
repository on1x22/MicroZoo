using MicroZoo.IdentityApi.Repositories;
using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Services
{
    public class UserRequirementsService : IUserRequirementsService
    {
        private readonly IUserRequirementsRepository _userRequirementsRepository;

        public UserRequirementsService(IUserRequirementsRepository userRequirementsRepository)
        {
            _userRequirementsRepository = userRequirementsRepository;
        }
        public async Task<List<string>> GetAllowedRequirementsOfUser(User user)
        {
            return await _userRequirementsRepository.GetAllowedRequirementsOfUser(user);
        }
    }
}
