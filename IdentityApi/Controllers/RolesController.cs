using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.IdentityApi.JwtFeatures;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Roles;
using MicroZoo.Infrastructure.Models.Users;
using MicroZoo.JwtConfiguration;

namespace MicroZoo.IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;
        private readonly IRoleRequirementsService _roleRequirementsService;
        private readonly ILogger<RolesController> _logger;
        private readonly JwtHandler _jwtHandler;

        public RolesController(IRolesService rolesService, 
            IRoleRequirementsService roleRequirementsService,
            ILogger<RolesController> logger,
            JwtHandler jwtHandler)
        {
            _rolesService = rolesService;
            _roleRequirementsService = roleRequirementsService;
            _logger = logger;
            _jwtHandler = jwtHandler;
        }

        [HttpGet]
        [Authorize(Policy = "IdentityApi.Read")]
        public async Task<IActionResult> GetAllRoles()
        {
            var response = await _rolesService.GetAllRolesAsync();

            return response.Roles != null 
                ? Ok(response.Roles) 
                : BadRequest(response.ErrorMessage);
        }

        [HttpGet("{roleId}")]
        [Authorize(Policy = "IdentityApi.Read")]        
        public async Task<IActionResult> GetRoleAsync(string roleId)
        {
            var response = await _rolesService.GetRoleAsync(roleId);

            return response.Role != null
                ? Ok(response.Role)
                : BadRequest(response.ErrorMessage);
        }

        [HttpPost]
        [Authorize(Policy = "IdentityApi.Create")]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleWithoutIdDto roleWithoutIdDto)
        {
            if (roleWithoutIdDto == null)
            {
                var remoteIpAddress = JwtExtensions.GetRemoteAddressFromHttpContext(HttpContext);
                _logger.LogWarning("Invalid RoleWithoutIdDto sent from address {remoteIpAddress}",
                    remoteIpAddress);
            }
            
            var response = await _rolesService.AddRoleAsync(roleWithoutIdDto);

            if (response.Role == null)
                _logger.LogInformation("An error occurred while adding role: {ErrorMessage}",
                    response.ErrorMessage);

            return response.Role != null
                ? Ok(response.Role)
                : BadRequest(response.ErrorMessage);
        }

        [HttpPut("{roleId}")]
        [Authorize(Policy = "IdentityApi.Update")]
        public async Task<IActionResult> UpdateRoleAsync(string roleId, 
            [FromBody] RoleWithoutIdDto roleWithoutIdDto)
        {
            if (roleWithoutIdDto == null)
            {
                var remoteIpAddress = JwtExtensions.GetRemoteAddressFromHttpContext(HttpContext);
                _logger.LogWarning("Invalid RoleWithoutIdDto sent from address {remoteIpAddress}",
                    remoteIpAddress);
            }

            var response = await _rolesService.UpdateRoleAsync(roleId, roleWithoutIdDto!);

            if (response.Role == null)
                _logger.LogInformation("An error occurred while updating role: {ErrorMessage}",
                    response.ErrorMessage);

            return response.Role != null
                ? Ok(response.Role)
                : BadRequest(response.ErrorMessage);
        }

        [HttpDelete("{roleId}")]
        [Authorize(Policy = "IdentityApi.Delete")]
        public async Task<IActionResult> SoftDeleteRoleAsync(string roleId)
        {
            var token = JwtExtensions.GetAccessTokenFromRequest(Request);
            var adminPrincipal = _jwtHandler.GetPrincipalFromToken(token);
            _logger.LogInformation("User {Name} tried to delete role with Id {roleId}",
                adminPrincipal.Identity!.Name, roleId);

            var response = await _rolesService.SoftDeleteRoleAsync(roleId);

            return response.Role != null
                ? Ok(response.Role)
                : BadRequest(response.ErrorMessage);
        }

        [HttpGet("{roleId}/with-requirements")]
        [Authorize(Policy = "IdentityApi.Read")]
        public async Task<IActionResult> GetRoleWithRequirementsAsync(string roleId)
        {
            var response = await _roleRequirementsService.GetRoleWithRequirementsAsync(roleId);

            return response.RoleWithRequirements != null
                ? Ok(response.RoleWithRequirements) 
                : BadRequest(response.ErrorMessage);
        }

        [HttpPut("{roleId}/with-requirements")]
        [Authorize(Policy = "IdentityApi.Update")]
        public async Task<IActionResult> UpdateRoleWithRequirementsAsync(string roleId,
            [FromBody] List<Guid> requirementIds)
        {
            var token = JwtExtensions.GetAccessTokenFromRequest(Request);
            var adminPrincipal = _jwtHandler.GetPrincipalFromToken(token);
            _logger.LogInformation("User {Name} tried to change requirements for role " +
                "with Id {roleId}",
                adminPrincipal.Identity!.Name, roleId);

            var response = await _roleRequirementsService.UpdateRoleWithRequirementsAsync(
                roleId, requirementIds);

            return response.RoleWithRequirements != null
                ? Ok(response.RoleWithRequirements)
                : BadRequest(response.ErrorMessage);
        }
    }
}
