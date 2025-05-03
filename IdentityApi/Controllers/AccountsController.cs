using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MicroZoo.EmailService;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.IdentityApi.Models.DTO;
using MicroZoo.IdentityApi.Models.Mappers;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Users;
using MicroZoo.JwtConfiguration;

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
            {
                _logger.LogWarning("Entered invalid user data for registration");
                return BadRequest();
            }

            if (!EmailValidator.Validate(userForRegistration.Email!))
            {
                _logger.LogWarning("Could not registrate user with invalid email: " +
                    "{@userForRegistration}", userForRegistration);

                var regRespDto = new RegistrationResponseDto
                {
                    IsSuccessfulRegistration = false,
                    Errors = new List<string>() { "Entered invalid email" }
                };

                return BadRequest(regRespDto);
            }

            var user = UserUpdater.ConvertFromUserForRegistrationDto(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password!);            
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                _logger.LogWarning("Error while creating user {Email}: {Errors}", 
                    userForRegistration.Email, errors);

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
            _logger.LogInformation("Message with confirmation successfully sent to email {Email}", 
                user.Email);

            await _userManager.AddToRoleAsync(user, "Visitor");
            _logger.LogInformation("For user {Email} granted role \"Visitor\"", user.Email);

            return StatusCode(201);
        }

        [HttpGet("email-confirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email,
                                                           [FromQuery] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User with email {emial} not found", email);
                return BadRequest("Invalid email confirmation request");
            }

            if (user.Deleted == true)
            {
                _logger.LogWarning("An attempt was made to create a user {email} that is " +
                    "marked as \"Deleted\"", email);
                return BadRequest("Invalid request");
            }

            var confirmResult = await _userManager.ConfirmEmailAsync(user,
                System.Web.HttpUtility.UrlDecode(token));
            if (!confirmResult.Succeeded)
            {
                _logger.LogWarning("Error while confirm user {email}", email);

                return BadRequest("Invalid email confirmation request");
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] UserForAuthenticationDto userForAuthentication)
        {
            var user = await _userManager.FindByEmailAsync(userForAuthentication.Email!);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} tried log in, but his doesn't exist " +
                    "in database", userForAuthentication.Email);

                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid authentication" });
            }

            if (user.Deleted == true)
            {
                _logger.LogWarning("An attempt was made to log in a user {Email} that is " +
                    "marked as \"Deleted\"", userForAuthentication.Email);

                return BadRequest("Invalid request");
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger.LogWarning("An attempt was made to log in a user {Email} that is " +
                    "locked out", userForAuthentication.Email);

                return Unauthorized(new AuthResponseDto { ErrorMessage = "The account is locked out" });
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogWarning("An attempt was made to log in a user {Email} that is " +
                    "email not confirmed", userForAuthentication.Email);

                return Unauthorized(new AuthResponseDto { ErrorMessage = "Email is not confirmed" });
            }

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

                    _logger.LogWarning("The user {Email} has reached the login attempt limit and " +
                        "has been locked out", userForAuthentication.Email);

                    return Unauthorized(new AuthResponseDto { ErrorMessage = "The account is locked out" });
                }

                _logger.LogWarning("The user {Email} entered invalid password", userForAuthentication.Email);

                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid authentication" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtHandler.CreateAccessToken(user, roles);
            var refreshToken = _jwtHandler.CreateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = _jwtHandler.GetRefreshTokenExpiryTimeSpanInDays();
            user.AccessFailedCount = 0;

            await _userManager.UpdateAsync(user);

            _logger.LogInformation("User with email {Email} is logged in", user.Email);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, AccessToken = accessToken,
                                                RefreshToken = refreshToken});
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email!);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} tried to forgot password, but " +
                    "his doesn't exist in database", forgotPasswordDto.Email);

                return BadRequest("Invalid request");
            }

            if (user.Deleted == true)
            {
                _logger.LogWarning("An attempt was made to forgot password a user {Email} " +
                    "that is marked as \"Deleted\"", forgotPasswordDto.Email);

                return BadRequest("Invalid request");
            }

            var token =await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                { "token", token },
                { "email", forgotPasswordDto.Email! }
            };

            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientUri!, param);

            var message = new Message([user.Email!], "Reset password token", callback);

            await _emailSender.SendEmailAsync(message);
            _logger.LogInformation("Successfully forgot password for user {Email}",
                forgotPasswordDto.Email);

            return Ok();
        }

        [HttpPost("reset-password")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email!);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} tried to reset password, but " +
                    "his doesn't exist in database", resetPasswordDto.Email);

                return BadRequest("Invalid request");
            }

            if (user.Deleted == true)
            {
                _logger.LogWarning("An attempt was made to reset password a user {Email} " +
                    "that is marked as \"Deleted\"", resetPasswordDto.Email);

                return BadRequest("Invalid request");
            }

            var result = await _userManager.ResetPasswordAsync(user, 
                System.Web.HttpUtility.UrlDecode(resetPasswordDto.Token!), resetPasswordDto.Password!);

            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                _logger.LogWarning("Error while reset password for user {Email}: {Errors}",
                    resetPasswordDto.Email, errors);

                return BadRequest(new { Errors = errors });
            }

            await _userManager.SetLockoutEndDateAsync(user, null);
            _logger.LogInformation("Successfully reset password for user {Email}", resetPasswordDto.Email);

            return Ok();
        }

        [HttpPost("lockout-user/{userId}")]
        [Authorize(Policy = "IdentityApi.Update")]
        public async Task<IActionResult> LockOutUser(string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var token = JwtExtensions.GetAccessTokenFromRequest(Request);
            var adminPrincipal = _jwtHandler.GetPrincipalFromToken(token);
            if (adminPrincipal == null)
            {
                _logger.LogWarning("An unexpected error occurred while determining the user " +
                    "who sent the request: {@adminPrincipal}", adminPrincipal);

                return BadRequest("Invalid request");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogInformation("User {Name} tried to lock out user with Id {userId}, but" +
                    " he doesn't exist in database", adminPrincipal.Identity!.Name, userId);

                return BadRequest("Invalid request");
            }

            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

            _logger.LogInformation("User {Name} successfully lock out user with Id {userId}",
                adminPrincipal.Identity!.Name, userId);

            return Ok($"User with Id = \"{userId}\" was locked out");
        }

        [HttpPost("unlock-user/{userId}")]
        [Authorize(Policy = "IdentityApi.Update")]
        public async Task<IActionResult> UnlockUser(string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var token = JwtExtensions.GetAccessTokenFromRequest(Request);
            var adminPrincipal = _jwtHandler.GetPrincipalFromToken(token);
            if (adminPrincipal == null)
            {
                _logger.LogWarning("An unexpected error occurred while determining the user " +
                    "who sent the request: {@adminPrincipal}", adminPrincipal);

                return BadRequest("Invalid request");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogInformation("User {Name} tried to unlock user with Id {userId}, but " +
                    "he doesn't exist in database", adminPrincipal.Identity!.Name, userId);

                return BadRequest("Invalid request");
            }
            await _userManager.SetLockoutEnabledAsync(user, false);
            await _userManager.SetLockoutEndDateAsync(user, null);

            _logger.LogInformation("User {Name} successfully unlock user with Id {userId}",
                adminPrincipal.Identity!.Name, userId);

            return Ok($"User with Id = \"{userId}\" was unlocked");
        }
    }
}
