using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Roles;
using MicroZoo.JwtConfiguration;

namespace MicroZoo.IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequirementsController : ControllerBase
    {
        private readonly IRequirementsService _requirementsService;
        private readonly ILogger<RequirementsController> _logger;
        private readonly IJwtHandler _jwtHandler;

        public RequirementsController(IRequirementsService requirementsService,
            ILogger<RequirementsController> logger,
            IJwtHandler jwtHandler)
        {
            _requirementsService = requirementsService;
            _logger = logger;
            _jwtHandler = jwtHandler;
        }

        [HttpGet]
        [Authorize(Policy = "IdentityApi.Read")]
        public async Task<IActionResult> GetAllRequirementsAsync() 
        { 
            var response = await _requirementsService.GetAllRequirementsAsync();

            return response.Requirements != null 
                ? Ok(response.Requirements)
                : BadRequest(response.ErrorMessage);
        }

        [HttpGet("{requirementId}")]
        [Authorize(Policy = "IdentityApi.Read")]
        public async Task<IActionResult> GetRequirementAsync(Guid requirementId)
        {
            var response = await _requirementsService.GetRequirementAsync(requirementId);

            return response.Requirement != null
                ? Ok(response.Requirement)
                : BadRequest(response.ErrorMessage);
        }

        [HttpPost]
        [Authorize(Policy = "IdentityApi.Create")]
        public async Task<IActionResult> AddRequirementAsync(
            [FromBody] RequirementWithoutIdDto requirementDto)
        {
            if (requirementDto == null)
            {
                var remoteIpAddress = JwtExtensions.GetRemoteAddressFromHttpContext(HttpContext);
                _logger.LogWarning("Invalid RequirementWithoutIdDto sent from address " +
                    "{remoteIpAddress}", remoteIpAddress);
                return BadRequest("Invalid request");
            }

            var response = await _requirementsService.AddRequirementAsync(requirementDto!);            

            if (response.Requirement == null)
            {
                _logger.LogInformation("An error occurred while adding requirement: " +
                    "{ErrorMessage}", response.ErrorMessage);
                return BadRequest(response.ErrorMessage);
            }            

            var adminPrincipal = _jwtHandler.GetPrincipalFromHttpRequest(Request);
            _logger.LogInformation("The user {Name} created new requirement {requirementDto}",
                adminPrincipal.Identity!.Name, requirementDto);

            return Ok(response.Requirement);
        }

        [HttpDelete("{requirementId}")]
        [Authorize(Policy = "IdentityApi.Delete")]
        public async Task<IActionResult> SoftDeleteRequirementAsync(Guid requirementId)
        {
            var adminPrincipal = _jwtHandler.GetPrincipalFromHttpRequest(Request);
            _logger.LogInformation("User {Name} tried to delete requirement with Id " +
                "{requirementId}", adminPrincipal.Identity!.Name, requirementId);

            var response = await _requirementsService.SoftDeleteRequirementAsync(requirementId);
           
            if (response.Requirement == null)
            {
                _logger.LogInformation("An error occurred while deleting role: {ErrorMessage}",
                    response.ErrorMessage);

                return BadRequest(response.ErrorMessage);
            }

            _logger.LogInformation("The user {Name} deleted requirement with Id {requirementId}",
                    adminPrincipal.Identity!.Name, requirementId);

            return Ok(response.Requirement);
        }
    }
}
