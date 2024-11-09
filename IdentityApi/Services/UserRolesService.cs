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

        public async Task<GetUserWithRolesResponse> UpdateUserWithRolesAsync(string userId, List<string> RoleIds)
        {
            var response = new GetUserWithRolesResponse();
            if (!Guid.TryParse(userId, out _))
            {
                response.ErrorMessage = "User Id is not Guid";
                return response;
            }

            foreach (var roleId in RoleIds)
            {
                if (!Guid.TryParse(roleId, out _))
                {
                    response.ErrorMessage = "Not all role Ids are Guid";
                    return response;
                }
            }

            var isSuccessfullyUpdated = await _userRolesRepository
                .UpdateUserWithRolesAsync(userId, RoleIds);

            if (isSuccessfullyUpdated == false)
            {
                response.ErrorMessage = $"User with Id {userId} does not exist";
                return response;
            }

            return await GetUserWithRolesAsync(userId);
        }
    }
}
