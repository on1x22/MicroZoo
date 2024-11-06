﻿using Microsoft.AspNetCore.Mvc;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Users;

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
        public async Task<IActionResult> GetAllUsersAsync()
        {            
            var response = await _usersService.GetAllUsersAsync();

            return response.Users != null
                ? Ok(response.Users)
                : BadRequest(response.ErrorMessage);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserAsync(string userId)
        {
            var response = await _usersService.GetUserAsync(userId);

            return response.User != null 
                ? Ok(response.User) 
                : BadRequest(response.ErrorMessage);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserAsync(string userId,[FromBody] User user)
        {
            var response = await _usersService.UpdateUserAsync(userId, user);

            return response.User != null 
                ? Ok(response.User) 
                : BadRequest(response.ErrorMessage);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(string userId)
        {
            var response = await _usersService.DeleteUserAsync(userId);

            return response.User != null
                ? Ok(response.User)
                : BadRequest(response.ErrorMessage);
        }

        [HttpGet("{userId}/with-roles")]
        public async Task<IActionResult> GetUserWithRolesAsync(string userId)
        {
            var response = await _userRolesService.GetUserWithRolesAsync(userId);

            return response.UserWithRoles != null
                ? Ok(response.UserWithRoles) 
                : BadRequest(response.ErrorMessage);
        }
    }
}
