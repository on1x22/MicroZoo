using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;
        private readonly IRoleRequirementsService _roleRequirementsService;

        public RolesController(IRolesService rolesService, 
            IRoleRequirementsService roleRequirementsService)
        {
            _rolesService = rolesService;
            _roleRequirementsService = roleRequirementsService;
        }

        [HttpGet]
        [Authorize(Roles = "Visitor")]
        public async Task<IActionResult> GetAllRoles()
        {
            var response = await _rolesService.GetAllRolesAsync();

            return response.Roles != null 
                ? Ok(response.Roles) 
                : BadRequest(response.ErrorMessage);
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleAsync(string roleId)
        {
            var response = await _rolesService.GetRoleAsync(roleId);

            return response.Role != null
                ? Ok(response.Role)
                : BadRequest(response.ErrorMessage);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleWithoutIdDto roleWithoutIdDto)
        {
            var response = await _rolesService.AddRoleAsync(roleWithoutIdDto);

            return response.Role != null
                ? Ok(response.Role)
                : BadRequest(response.ErrorMessage);
        }

        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRoleAsync(string roleId, 
            [FromBody] RoleWithoutIdDto roleWithoutIdDto)
        {
            var response = await _rolesService.UpdateRoleAsync(roleId, roleWithoutIdDto);

            return response.Role != null
                ? Ok(response.Role)
                : BadRequest(response.ErrorMessage);
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRoleAsync(string roleId)
        {
            var response = await _rolesService.DeleteRoleAsync(roleId);

            return response.Role != null
                ? Ok(response.Role)
                : BadRequest(response.ErrorMessage);
        }

        [HttpGet("{roleId}/with-requirements")]
        public async Task<IActionResult> GetRoleWithRequirementsAsync(string roleId)
        {
            var response = await _roleRequirementsService.GetRoleWithRequirementsAsync(roleId);

            return response.RoleWithRequirements != null
                ? Ok(response.RoleWithRequirements) 
                : BadRequest(response.ErrorMessage);
        }

        [HttpPut("{roleId}/with-requirements")]
        public async Task<IActionResult> UpdateRoleWithRequirementsAsync(string roleId,
            [FromBody] List<Guid> requirementIds)
        {
            var response = await _roleRequirementsService.UpdateRoleWithRequirementsAsync(
                roleId, requirementIds);

            return response.RoleWithRequirements != null
                ? Ok(response.RoleWithRequirements)
                : BadRequest(response.ErrorMessage);
        }
    }
}
