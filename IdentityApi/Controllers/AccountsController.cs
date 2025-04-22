using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MicroZoo.EmailService;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.IdentityApi.Models.DTO;
using MicroZoo.IdentityApi.Models.Mappers;
using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly JwtHandler _jwtHandler;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(UserManager<User> userManager, IEmailSender emailSender, 
            JwtHandler jwtHandler, ILogger<AccountsController> logger)
        {
            _userManager = userManager;            
            _emailSender = emailSender;
            _jwtHandler = jwtHandler;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration == null)
                return BadRequest();

            var user = UserUpdater.ConvertFromUserForRegistrationDto(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password!);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token},
                {"email", user.Email}
            };

            var callback = QueryHelpers.AddQueryString(userForRegistration.ClientUri!, param);

            var message = new Message([user.Email!], "Email confirmation token", callback);

            await _emailSender.SendEmailAsync(message);

            await _userManager.AddToRoleAsync(user, "Visitor");

            return StatusCode(201);
        }

        [HttpGet("email-confirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email,
                                                           [FromQuery] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) 
                return BadRequest("Invalid email confirmation request");

            if (user.Deleted == true)
                return BadRequest("Invalid request");

            var confirmResult = await _userManager.ConfirmEmailAsync(user,
                System.Web.HttpUtility.UrlDecode(token));
            if (!confirmResult.Succeeded) 
                return BadRequest("Invalid email confirmation request");

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] UserForAuthenticationDto userForAuthentication)
        {
            var user = await _userManager.FindByEmailAsync(userForAuthentication.Email!);
            if (user == null)
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid authentication" });

            if (user.Deleted == true)
                return BadRequest("Invalid request");

            if (await _userManager.IsLockedOutAsync(user))
                return Unauthorized(new AuthResponseDto { ErrorMessage = "The account is locked out"});

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Email is not confirmed"});

            if (!await _userManager.CheckPasswordAsync(user, userForAuthentication.Password!))
            {
                await _userManager.AccessFailedAsync(user);
                if (await _userManager.IsLockedOutAsync(user))
                {
                    var content = "Your account is locked out. If you want to reset the password, " +
                        "you can use the Forgot password link on the Login page";

                    var message = new Message([userForAuthentication.Email!],
                        "Locked out account information", content);

                    await _emailSender.SendEmailAsync(message);

                    return Unauthorized(new AuthResponseDto { ErrorMessage = "The account is locked out" });
                }

                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid authentication" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtHandler.CreateAccessToken(user, roles);
            var refreshToken = _jwtHandler.CreateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = _jwtHandler.GetRefreshTokenExpiryTimeSpanInDays();
            user.AccessFailedCount = 0;

            await _userManager.UpdateAsync(user);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, AccessToken = accessToken,
                                                RefreshToken = refreshToken});
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(forgotPassword.Email!);
            if (user == null)
                return BadRequest("Invalid request");

            if (user.Deleted == true)
                return BadRequest("Invalid request");

            var token =await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                { "token", token },
                { "email", forgotPassword.Email! }
            };

            var callback = QueryHelpers.AddQueryString(forgotPassword.ClientUri!, param);

            var message = new Message([user.Email!], "Reset password token", callback);

            await _emailSender.SendEmailAsync(message);

            return Ok();
        }

        [HttpPost("reset-password")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(resetPassword.Email!);
            if (user == null)
                return BadRequest("Invalid request");

            if (user.Deleted == true)
                return BadRequest("Invalid request");

            var result = await _userManager.ResetPasswordAsync(user, 
                System.Web.HttpUtility.UrlDecode(resetPassword.Token!), resetPassword.Password!);

            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new { Errors = errors });
            }

            await _userManager.SetLockoutEndDateAsync(user, null);

            return Ok();
        }

        [HttpPost("lockout-user/{userId}")]
        [Authorize(Policy = "IdentityApi.Update")]
        public async Task<IActionResult> LockOutUser(string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("Invalid request");

            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            return Ok($"User with Id = \"{userId}\" was locked out");
        }

        [HttpPost("unlock-user/{userId}")]
        [Authorize(Policy = "IdentityApi.Update")]
        public async Task<IActionResult> UnlockUser(string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("Invalid request");

            await _userManager.SetLockoutEnabledAsync(user, false);
            await _userManager.SetLockoutEndDateAsync(user, null);
            return Ok($"User with Id = \"{userId}\" was unlocked");
        }
    }
}
