using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.IdentityApi.Services
{
    public class UsersRequestReceivingService : IUsersRequestReceivingService
    {
        private readonly IUsersService _usersService;

        public UsersRequestReceivingService(IUsersService usersService)
        {
            _usersService = usersService;
        }
        public async Task<GetUsersResponse> GetAllUsersAsync() =>
            await _usersService.GetAllUsersAsync();

        public async Task<GetUserResponse> GetUserAsync(string userId)
        {
            var response = new GetUserResponse();

            if(!Guid.TryParse(userId, out Guid userIdGuid))
            {
                response.ErrorMessage = "User Id is not Guid";
                return response;
            }

            response = await _usersService.GetUserAsync(userId);

            return response;
        }
    }
}
