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
        private readonly ILogger<TokensController> _logger;

        public TokensController(UserManager<User> userManager, JwtHandler jwtHandler,
            ILogger<TokensController> logger)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _logger = logger;
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress!.ToString();
            var remotePort = HttpContext.Connection.RemotePort!.ToString();

            if (tokenModel == null)
            {                
                _logger.LogWarning("Invalid TokenModel sent from address {remoteIpAddress}, " +
                    "port {remotePort}", remoteIpAddress, remotePort);

                return BadRequest("Invalid client request");
            }

            var accessToken = tokenModel.AccessToken;
            var refreshToken = tokenModel.RefreshToken;

            var principal = _jwtHandler.GetPrincipalFromExpiredToken(accessToken!);
            if (principal == null)
            {
                _logger.LogWarning("An unexpected error occurred while determining the user " +
                    "who sent the request from address {remoteIpAddress}, port {remotePort}",
                    remoteIpAddress, remotePort);

                return BadRequest("Invalid access token or refresh token");
            }

            string username = principal.Identity!.Name!;

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                _logger.LogWarning("User with email {username} tried to refresh token, " +
                    "but his doesn't exist in database", username);

                return BadRequest("Invalid access token or refresh token");
            }

            if (user.RefreshToken != refreshToken)
            {
                _logger.LogWarning("User with email {username} tried to refresh token, " +
                    "but refresh token in request doesn't match with refresh token in database", username);

                return BadRequest("Invalid access token or refresh token");
            }

            if (user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                _logger.LogWarning("User with email {username} tried to refresh token, " +
                    "but expiry time of refresh token less then current DateTime", username);

                return BadRequest("Invalid access token or refresh token");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _jwtHandler.CreateAccessToken(user, roles);
            var newRefreshToken = _jwtHandler.CreateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = _jwtHandler.GetRefreshTokenExpiryTimeSpanInDays();

            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Successfully refreshed access token and refresh token for " +
                "user with email {username}", username);

            return Ok(new TokenModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("revoke-refresh-token")]
        [Authorize]
        public async Task<IActionResult> RevokeRefreshToken()
        {
            var userName = User.Identity!.Name;
            var user = await _userManager.FindByNameAsync(userName!);

            if (user == null)
            {
                _logger.LogWarning("User with email {userName} tried to revoke refresh token, " +
                    "but his doesn't exist in database", userName);

                return BadRequest("Invalid username");
            }

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Successfully revoked refresh token for " +
                "user with email {userName}", userName);

            return NoContent();
        }
    }
}
