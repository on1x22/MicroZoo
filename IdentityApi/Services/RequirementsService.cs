using MicroZoo.IdentityApi.Repositories;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Services
{
    public class RequirementsService : IRequirementsService
    {
        private readonly IRequirementsRepository _repository;

        public RequirementsService(IRequirementsRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<GetRequirementsResponse> GetAllRequirementsAsync() =>        
            new GetRequirementsResponse()
            {
                Requirements = await _repository.GetAllRequirementsAsync()
            };
        
        public async Task<GetRequirementResponse> GetRequirementAsync(Guid requirementId)
        {
            var response = new GetRequirementResponse();

            var requirement = await _repository.GetRequirementAsync(requirementId);

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

            var addedRequirement = await _repository.AddRequirementAsync(newRequirement);
            if (addedRequirement == null) 
            {
                response.ErrorMessage = $"Requirement with name {newRequirement.Name} already exist";
                return response;
            }

            response.Requirement = newRequirement;
            return response;
        }

        public async Task<GetRequirementResponse> DeleteRequirementAsync(Guid requirementId)
        {
            var response = new GetRequirementResponse();

            var deletedRequirement = await _repository.DeleteRequirementAsync(requirementId);
            if(deletedRequirement == null)
            {
                response.ErrorMessage = $"Requirement with Id {requirementId} does not exist";
                return response;
            }

            response.Requirement = deletedRequirement;
            return response;
        }        
    }
}
