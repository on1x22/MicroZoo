using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.IdentityApi.Models.DTO;
using MicroZoo.IdentityApi.Services;
using MicroZoo.JwtConfiguration;

namespace MicroZoo.IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IUserRolesService _userRolesService;
        private readonly JwtHandler _jwtHandler;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUsersService usersService, IUserRolesService userRolesService,
            JwtHandler jwtHandler, ILogger<UsersController> logger)
        {
            _usersService = usersService;
            _userRolesService = userRolesService;
            _jwtHandler = jwtHandler;
            _logger = logger;
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
            var token = JwtExtensions.GetAccessTokenFromRequest(Request);
            var adminPrincipal = _jwtHandler.GetPrincipalFromToken(token);
            _logger.LogInformation("User {Name} tried to update data about user with Id {userId}",
                adminPrincipal.Identity!.Name, userId);

            var response = await _usersService.UpdateUserAsync(userId, userForUpdateDto);

            return response.User != null 
                ? Ok(response.User) 
                : BadRequest(response.ErrorMessage);
        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = "IdentityApi.Delete")]
        public async Task<IActionResult> SoftDeleteUserAsync(string userId)
        {
            var token = JwtExtensions.GetAccessTokenFromRequest(Request);
            var adminPrincipal = _jwtHandler.GetPrincipalFromToken(token);
            _logger.LogInformation("User {Name} tried to delete user with Id {userId}",
                adminPrincipal.Identity!.Name, userId);

            var response = await _usersService.SoftDeleteUserAsync(userId);

            return response.User != null
                ? Ok(response.User)
                : BadRequest(response.ErrorMessage);
        }

        [HttpGet("{userId}/with-roles")]
        [Authorize(Policy = "IdentityApi.Read")]
        public async Task<IActionResult> GetUserWithRolesAsync(string userId)
        {
            var response = await _userRolesService.GetUserWithRolesAsync(userId);

            return response.UserWithRoles != null
                ? Ok(response.UserWithRoles)
                : BadRequest(response.ErrorMessage);
        }

        [HttpPut("{userId}/with-roles")]
        [Authorize(Policy = "IdentityApi.Update")]
        public async Task<IActionResult> UpdateUserWithRolesAsync(string userId,
            [FromBody] List<string> roleIds)
        {
            var token = JwtExtensions.GetAccessTokenFromRequest(Request);
            var adminPrincipal = _jwtHandler.GetPrincipalFromToken(token);
            _logger.LogInformation("User {Name} tried to change roles for user with Id {userId}",
                adminPrincipal.Identity!.Name, userId);

            var response = await _userRolesService.UpdateUserWithRolesAsync(userId, roleIds);

            return response.UserWithRoles != null
                ? Ok(response.UserWithRoles)
                : BadRequest(response.ErrorMessage);
        }
    }
}
