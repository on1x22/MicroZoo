using MicroZoo.IdentityApi.Repositories;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Services
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _repository;

        public RolesService(IRolesRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetRolesResponse> GetAllRolesAsync() =>
            new GetRolesResponse
            {
                Roles = await _repository.GetAllRolesAsync()
            };

        public async Task<GetRoleResponse> GetRoleAsync(string roleId)
        {
            var response = new GetRoleResponse();
            if(!Guid.TryParse(roleId, out _))
            {
                response.ErrorMessage = "Role Id is not Guid";
                return response;
            }

            var role = await _repository.GetRoleAsync(roleId);

            if (role == null)
            {
                response.ErrorMessage = $"Role with Id {roleId} does not exist";
                return response;
            }

            response.Role = role;
            return response;
        }

        public async Task<GetRoleResponse> AddRoleAsync(RoleWithoutIdDto roleWithoutIdDto)
        {
            var response = new GetRoleResponse();
            if (roleWithoutIdDto == null)
            {
                response.ErrorMessage = "New role must be not null";
                return response;
            }

            var newRole = new Role();
            newRole.SetValues(roleWithoutIdDto);

            var addedRole = await _repository.AddRoleAsync(newRole);
            if (addedRole == null)
            {
                response.ErrorMessage = "Not expected error during creation role";
                return response;
            }

            response.Role = addedRole;
            
            return response;
        }

        public async Task<GetRoleResponse> UpdateRoleAsync(string roleId, 
            RoleWithoutIdDto roleWithoutIdDto)
        {
            var response = new GetRoleResponse();
            if (!Guid.TryParse(roleId, out _))
            {
                response.ErrorMessage = "Role Id is not Guid";
                return response;
            }

            var updatedRole = new Role();
            updatedRole.SetValues(roleWithoutIdDto);

            response.Role = await _repository.UpdateRoleAsync(roleId, updatedRole);
            if(response.Role == null)            
                response.ErrorMessage = $"Role with Id {roleId} does not exist";
            
            return response;
        }

        public async Task<GetRoleResponse> DeleteRoleAsync(string roleId)
        {
            var response = new GetRoleResponse();
            if (!Guid.TryParse(roleId, out _))
            {
                response.ErrorMessage = "Role Id is not Guid";
                return response;
            }

            response.Role = await _repository.DeleteRoleAsync(roleId);

            return response;
        }
    }
}
