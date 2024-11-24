using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.IdentityApi.Models;
using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtHandler _jwtHandler;

        public TokensController(UserManager<User> userManager, JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel == null)
                return BadRequest("Invalid client request");

            var accessToken = tokenModel.AccessToken;
            var refreshToken = tokenModel.RefreshToken;

            var principal = _jwtHandler.GetPrincipalFromExpiredToken(accessToken!);
            if (principal == null)
                return BadRequest("Invalid access token or refresh token");

            string username = principal.Identity!.Name!;

            var user = await _userManager.FindByNameAsync(username);
            if (user == null || user.RefreshToken != refreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid access token or refresh token");

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _jwtHandler.CreateAccessToken(user, roles);
            var newRefreshToken = _jwtHandler.CreateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = _jwtHandler.GetRefreshTokenExpiryTimeSpanInDays();

            await _userManager.UpdateAsync(user);

            return Ok(new TokenModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("revokerefreshtoken")]
        [Authorize]
        public async Task<IActionResult> RevokeRefreshToken()
        {
            var userName = User.Identity!.Name;
            var user = await _userManager.FindByNameAsync(userName!);

            if (user == null)
                return BadRequest("Invalid username");

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return NoContent();
        }
    }
}
