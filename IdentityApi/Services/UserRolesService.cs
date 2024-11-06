using MicroZoo.IdentityApi.Repositories;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.IdentityApi.Services
{
    public class UserRolesService : IUserRolesService
    {
        private readonly IUserRolesRepository _userRolesRepository;

        public UserRolesService(IUserRolesRepository userRolesRepository)
        {
            _userRolesRepository = userRolesRepository;
        }

        public async Task<GetUserWithRolesResponse> GetUserWithRolesAsync(string userId)
        {
            var response = new GetUserWithRolesResponse();
            if (!Guid.TryParse(userId, out _))
            {
                response.ErrorMessage = "User Id is not Guid";
                return response;
            }

            var userWithRoles = await _userRolesRepository.GetUserWithRolesAsync(userId);

            if (userWithRoles == null)
            {
                response.ErrorMessage = $"User with Id {userId} does not exist";
                return response;
            }

            response.UserWithRoles = userWithRoles;
            return response;
        }
    }
}
