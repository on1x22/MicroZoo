using MicroZoo.IdentityApi.Repositories;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Services
{
    public class RequirementsService : IRequirementsService
    {
        private readonly IRequirementsRepository _requirementRepository;
        private readonly IRoleRequirementsService _roleRequirementsService;

        public RequirementsService(IRequirementsRepository requirementRepository,
            IRoleRequirementsService roleRequirementsService)
        {
            _requirementRepository = requirementRepository;
            _roleRequirementsService = roleRequirementsService;
        }
        
        public async Task<GetRequirementsResponse> GetAllRequirementsAsync() =>        
            new GetRequirementsResponse()
            {
                Requirements = await _requirementRepository.GetAllRequirementsAsync()
            };
        
        public async Task<GetRequirementResponse> GetRequirementAsync(Guid requirementId)
        {
            var response = new GetRequirementResponse();

            var requirement = await _requirementRepository.GetRequirementAsync(requirementId);

            if (requirement == null)
            {
                response.ErrorMessage = $"Requirement with Id {requirementId} does not exist";
                return response;
            }

            response.Requirement = requirement;
            return response;
        }

        public async Task<GetRequirementResponse> AddRequirementAsync(RequirementWithoutIdDto requirementDto)
        {
            var response = new GetRequirementResponse();
            if (requirementDto == null)
            {
                response.ErrorMessage = "New requirement must be not null";
                return response;
            }

            if (string.IsNullOrEmpty(requirementDto.Name))
            {
                response.ErrorMessage = "Name of new requirement must be not null or empty";
                return response;
            }

            var newRequirement = new Requirement();
            newRequirement.SetValues(requirementDto);

            var addedRequirement = await _requirementRepository.AddRequirementAsync(newRequirement);
            if (addedRequirement == null) 
            {
                response.ErrorMessage = $"Requirement with name {newRequirement.Name} already exist";
                return response;
            }

            response.Requirement = newRequirement;
            return response;
        }

        public async Task<GetRequirementResponse> SoftDeleteRequirementAsync(Guid requirementId)
        {
            var response = new GetRequirementResponse();

            var requirementForDelete = await _requirementRepository.GetRequirementAsync(requirementId);
            if (requirementForDelete == null)
            {
                response.ErrorMessage = $"Requirement with Id {requirementId} does not exist";
                return response;
            }

            await _roleRequirementsService.DeleteRoleRequirementsAsync(requirementId);
            /*var deletedRequirement*/ 
            response.Requirement = await _requirementRepository.SoftDeleteRequirementAsync(requirementForDelete);
            /*if(deletedRequirement == null)
            {
                response.ErrorMessage = $"Requirement with Id {requirementId} does not exist";
                return response;
            }

            response.Requirement = deletedRequirement;*/
            return response;
        }        
    }
}
