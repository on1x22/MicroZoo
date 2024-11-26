using MicroZoo.IdentityApi.Repositories;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Users;

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
            if (!Guid.TryParse(userId, out _))
            {
                response.ErrorMessage = "User Id is not Guid";
                return response;
            }

            var user = await _userRepository.GetUserAsync(userId);

            if (user == null) 
            {
                response.ErrorMessage = $"User with Id {userId} does not exist";
                return response;
            }
            
            response.User = user;
            return response;
        }

        public async Task<GetUserResponse> UpdateUserAsync(string userId, User user)
        {
            var response = new GetUserResponse();
            if (!Guid.TryParse(userId, out _))
            {
                response.ErrorMessage = "User Id is not Guid";
                return response;
            }

            if (user == null)
            {
                response.ErrorMessage = "Invalid data for update";
                return response;
            }

            var userForUpdate = await _userRepository.GetUserAsync(userId);

            if (userForUpdate == null)
            {
                response.ErrorMessage = $"User with Id {userId} does not exist";
                return response;
            }

            response.User = await _userRepository.UpdateUserAsync(userId, user);

            if (response.User == null)
            {
                response.ErrorMessage = "Inner server error";
                return response;
            }

            return response;
        }

        public async Task <GetUserResponse> SoftDeleteUserAsync(string userId)
        {
            var response = new GetUserResponse();
            if (!Guid.TryParse(userId, out _))
            {
                response.ErrorMessage = "User Id is not Guid";
                return response;
            }

            var userForDelete = await _userRepository.GetUserAsync(userId);

            if (userForDelete == null)
            {
                response.ErrorMessage = $"User with Id {userId} does not exist";
                return response;
            }

            response.User = await _userRepository.SoftDeleteUserAsync(userForDelete);

            return response;
        }
    }
}
