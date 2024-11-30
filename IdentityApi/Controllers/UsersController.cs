using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.IdentityApi.Models.DTO;
using MicroZoo.IdentityApi.Services;

namespace MicroZoo.IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IUserRolesService _userRolesService;

        public UsersController(IUsersService usersService, IUserRolesService userRolesService)
        {
            _usersService = usersService;
            _userRolesService = userRolesService;
        }

        [HttpGet]
        [Authorize(Policy = "IdentityApi.Read")]
        public async Task<IActionResult> GetAllUsersAsync()
        {            
            var response = await _usersService.GetAllUsersAsync();

            return response.Users != null
                ? Ok(response.Users)
                : BadRequest(response.ErrorMessage);
        }

        [HttpGet("{userId}")]
        [Authorize(Policy = "IdentityApi.Read")]
        public async Task<IActionResult> GetUserAsync(string userId)
        {
            var response = await _usersService.GetUserAsync(userId);

            return response.User != null 
                ? Ok(response.User) 
                : BadRequest(response.ErrorMessage);
        }

        [HttpPut("{userId}")]
        [Authorize(Policy = "IdentityApi.Update")]
        public async Task<IActionResult> UpdateUserAsync(string userId,
            [FromBody] UserForUpdateDto userForUpdateDto)
        {
            var response = await _usersService.UpdateUserAsync(userId, userForUpdateDto);

            return response.User != null 
                ? Ok(response.User) 
                : BadRequest(response.ErrorMessage);
        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = "IdentityApi.Delete")]
        public async Task<IActionResult> SoftDeleteUserAsync(string userId)
        {
            var response = await _usersService.SoftDeleteUserAsync(userId);

            return response.User != null
                ? Ok(response.User)
                : BadRequest(response.ErrorMessage);
        }

        [HttpGet("{userId}/with-roles")]
        [Authorize(Policy = "IdentityApi.Read")]
        public async Task<IActionResult> GetUserWithRolesAsync_v2(string userId)
        {
            var response = await _userRolesService.GetUserWithRolesAsync(userId);

            return response.UserWithRoles != null
                ? Ok(response.UserWithRoles)
                : BadRequest(response.ErrorMessage);
        }

        [HttpPut("{userId}/with-roles")]
        [Authorize(Policy = "IdentityApi.Update")]
        public async Task<IActionResult> UpdateUserWithRolesAsync_v2(string userId,
            [FromBody] List<string> roleIds)
        {
            var response = await _userRolesService.UpdateUserWithRolesAsync(userId, roleIds);

            return response.UserWithRoles != null
                ? Ok(response.UserWithRoles)
                : BadRequest(response.ErrorMessage);
        }
    }
}
