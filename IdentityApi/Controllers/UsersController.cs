using Microsoft.AspNetCore.Mvc;
using MicroZoo.IdentityApi.Services;

namespace MicroZoo.IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRequestReceivingService _usersRequestReceivingService;

        public UsersController(IUsersRequestReceivingService usersRequestReceivingService)
        {
            _usersRequestReceivingService = usersRequestReceivingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var response = await _usersRequestReceivingService.GetAllUsersAsync();

            return response.Users != null
                ? Ok(response.Users)
                : NotFound(response.ErrorMessage);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserAsync(string userId)
        {
            var response = await _usersRequestReceivingService.GetUserAsync(userId);

            return response.User != null 
                ? Ok(response.User) 
                : NotFound(response.ErrorMessage);
        }
    }
}
