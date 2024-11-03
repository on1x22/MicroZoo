using MicroZoo.IdentityApi.Repositories;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.IdentityApi.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _userRepository;

        public UsersService(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUsersResponse> GetAllUsersAsync() =>
            new GetUsersResponse
            {
                Users = await _userRepository.GetAllUsersAsync()
            };

        public async Task<GetUserResponse> GetUserAsync(string userId)
        {
            var response = new GetUserResponse();

            var user = await _userRepository.GetUserAsync(userId);

            if (user == null) 
            {
                response.ErrorMessage = $"User with Id {userId} does not exist";
                return response;
            }
            
            response.User = user;
            return response;
        }
        
    }
}
