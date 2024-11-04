using Microsoft.AspNetCore.Mvc;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {            
            var response = await _usersService.GetAllUsersAsync();

            return response.Users != null
                ? Ok(response.Users)
                : NotFound(response.ErrorMessage);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserAsync(string userId)
        {
            var response = await _usersService.GetUserAsync(userId);

            return response.User != null 
                ? Ok(response.User) 
                : NotFound(response.ErrorMessage);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserAsync(string userId,[FromBody] User user)
        {
            var response = await _usersService.UpdateUserAsync(userId, user);

            return response.User != null 
                ? Ok(response.User) 
                : NotFound(response.ErrorMessage);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(string userId)
        {
            var response = await _usersService.DeleteUserAsync(userId);

            return response.User != null
                ? Ok(response.User)
                : NotFound(response.ErrorMessage);
        }
    }
}
