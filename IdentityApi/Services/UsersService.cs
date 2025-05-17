using MicroZoo.IdentityApi.Models.DTO;
using MicroZoo.IdentityApi.Repositories;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.IdentityApi.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _userRepository;
        private readonly IUserRolesService _userRolesService;
        private readonly ILogger<UsersService> _logger;

        public UsersService(IUsersRepository userRepository,
            IUserRolesService userRolesService, ILogger<UsersService> logger)
        {
            _userRepository = userRepository;
            _userRolesService = userRolesService;
            _logger = logger;
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

        public async Task<GetUserResponse> UpdateUserAsync(string userId, 
            UserForUpdateDto userForUpdateDto)
        {
            var response = new GetUserResponse();
            if (!Guid.TryParse(userId, out _))
            {
                response.ErrorMessage = "User Id is not Guid";
                return response;
            }

            if (userForUpdateDto == null)
            {
                response.ErrorMessage = "Invalid data for update";
                return response;
            }

            response.User = await _userRepository.UpdateUserAsync(userId, userForUpdateDto);

            if (response.User == null)
            {
                response.ErrorMessage = $"User with Id {userId} does not exist";
                return response;
            }

            _logger.LogInformation("Information about user with Id {userId} has been " +
                "updated: {@userForUpdateDto}", userId, userForUpdateDto);

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
            
            await _userRolesService.DeleteUserRolesByUserIdAsync(userId);
            response.User = await _userRepository.SoftDeleteUserAsync(userForDelete);

            _logger.LogInformation("User with Id {userId} has been marked as \"Deleted\"", userId);

            return response;
        }
    }
}
