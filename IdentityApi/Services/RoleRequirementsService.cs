using MicroZoo.IdentityApi.Repositories;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Services
{
    public class RoleRequirementsService : IRoleRequirementsService
    {
        private readonly IRoleRequirementsRepository _roleRequirementsRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IRequirementsRepository _requirementsRepository;

        public RoleRequirementsService(IRoleRequirementsRepository roleRequirementsRepository,
            IRolesRepository rolesRepository,
            IRequirementsRepository requirementsRepository)
        {
            _roleRequirementsRepository = roleRequirementsRepository;
            _rolesRepository = rolesRepository;
            _requirementsRepository = requirementsRepository;
        }

        public async Task<GetRoleWithRequirementsResponse> GetRoleWithRequirementsAsync(
            string roleId)
        {
            var response = new GetRoleWithRequirementsResponse();
            if (!Guid.TryParse(roleId, out _))
            {
                response.ErrorMessage = "Role Id is not Guid";
                return response;
            }

            var selectedRole = await _rolesRepository.GetRoleAsync(roleId);
            if (selectedRole == null)
            {
                response.ErrorMessage = $"Role with Id {roleId} does not exist";
                return response;
            }

            var requirementsOfRole = await _roleRequirementsRepository
                .GetRequirementsOfRoleAsync(roleId);

            var roleWithRequirements = selectedRole.ConvertToRoleWithRequirements();
            roleWithRequirements.Requirements = requirementsOfRole;

            response.RoleWithRequirements = roleWithRequirements;
            
            return response;
        }

        public async Task<GetRoleWithRequirementsResponse> UpdateRoleWithRequirementsAsync(
            string roleId, List<Guid> requirementIds)
        {
            var response = new GetRoleWithRequirementsResponse();
            if (!Guid.TryParse(roleId, out _))
            {
                response.ErrorMessage = "Role Id is not Guid";
                return response;
            }

            if (requirementIds == null)
            {
                response.ErrorMessage = "Invalid list of requirements Ids";
                return response;
            }

            var areAllRequirementIdsExistInDb = _requirementsRepository
                .CheckEntriesAreExistInDatabase(requirementIds);
            if (!areAllRequirementIdsExistInDb)
            {
                response.ErrorMessage = "Invalid list of requirements Ids";
                return response;
            }

            var selectedRole = await _rolesRepository.GetRoleAsync(roleId);
            if (selectedRole == null)
            {
                response.ErrorMessage = $"Role with Id {roleId} does not exist";
                return response;
            }

            var isSuccessfullyDeleted = await _roleRequirementsRepository
                .DeleteRoleRequirementsByRoleIdAsync(roleId);
            if (!isSuccessfullyDeleted)
            {
                response.ErrorMessage = "Innser server error";
                return response;
            }

            var newRoleRequirements = new List<RoleRequirement>();
            foreach (var requirementId in requirementIds)
            {
                var roleRequirement = new RoleRequirement()
                {
                    RoleId = roleId,
                    RequirementId = requirementId
                };
                newRoleRequirements.Add(roleRequirement);
            }

            var isSuccessfullyAdded = await _roleRequirementsRepository
                .AddRoleRequirementsAsync(newRoleRequirements);
            if (!isSuccessfullyAdded)
            {
                response.ErrorMessage = "Innser server error";
                return response;
            }

            return await GetRoleWithRequirementsAsync(roleId);
        }

        public async Task<bool> DeleteRoleRequirementsByRequirementIdAsync(Guid requirementId)
        {
            var isSuccessfulyDeleted = await _roleRequirementsRepository
                .DeleteRoleRequirementsByRequirementIdAsync(requirementId);

            if (!isSuccessfulyDeleted) 
                return false;

            return true;
        }

        public async Task<bool> DeleteRoleRequirementsByRoleIdAsync(string roleId)
        {
            var isSuccessfulyDeleted = await _roleRequirementsRepository
                .DeleteRoleRequirementsByRoleIdAsync(roleId);

            if (!isSuccessfulyDeleted)
                return false;

            return true;
        }
    }
}
