using MicroZoo.IdentityApi.Repositories;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Services
{
    public class RoleRequirementsService : IRoleRequirementsService
    {
        private readonly IRoleRequirementsRepository _roleRequirementsRepository;

        public RoleRequirementsService(IRoleRequirementsRepository roleRequirementsRepository)
        {
            _roleRequirementsRepository = roleRequirementsRepository;
        }

        public async Task<GetRoleWithRequirementsResponse> GetRoleWithRequirementsAsync(string roleId)
        {
            var response = new GetRoleWithRequirementsResponse();
            if (!Guid.TryParse(roleId, out _))
            {
                response.ErrorMessage = "Role Id is not Guid";
                return response;
            }

            var roleWithRequirements = await _roleRequirementsRepository
                .GetRoleWithRequirementsAsync(roleId);

            if (roleWithRequirements == null)
            {
                response.ErrorMessage = $"Role with Id {roleId} does not exist";
                return response;
            }

            response.RoleWithRequirements = roleWithRequirements;
            return response;
        }
    }
}
