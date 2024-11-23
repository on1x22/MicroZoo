using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroZoo.IdentityApi.Services;
using MicroZoo.Infrastructure.Models.Roles;
using System.Linq.Expressions;

namespace MicroZoo.IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequirementsController : ControllerBase
    {
        private readonly IRequirementsService _requirementsService;

        public RequirementsController(IRequirementsService requirementsService)
        {
            _requirementsService = requirementsService;
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
            var response = await _requirementsService.AddRequirementAsync(requirementDto);

            return response.Requirement != null
                ? Ok(response.Requirement)
                : BadRequest(response.ErrorMessage);
        }

        [HttpDelete("{requirementId}")]
        [Authorize(Policy = "IdentityApi.Delete")]
        public async Task<IActionResult> DeleteRequirementasync(Guid requirementId)
        {
            var response = await _requirementsService.DeleteRequirementAsync(requirementId);

            return response.Requirement != null
                ? Ok(response.Requirement)
                : BadRequest(response.ErrorMessage);
        }
    }
}
