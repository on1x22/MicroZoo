﻿using MicroZoo.IdentityApi.Repositories;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Services
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IUserRolesService _userRolesService;
        private readonly IRoleRequirementsService _roleRequirementsService;

        public RolesService(IRolesRepository rolesRepository,            
            IUserRolesService userRolesService,
            IRoleRequirementsService roleRequirementsService)
        {
            _rolesRepository = rolesRepository;
            _userRolesService = userRolesService;
            _roleRequirementsService = roleRequirementsService;
        }

        public async Task<GetRolesResponse> GetAllRolesAsync() =>
            new GetRolesResponse
            {
                Roles = await _rolesRepository.GetAllRolesAsync()
            };

        public async Task<GetRoleResponse> GetRoleAsync(string roleId)
        {
            var response = new GetRoleResponse();
            if(!Guid.TryParse(roleId, out _))
            {
                response.ErrorMessage = "Role Id is not Guid";
                return response;
            }

            var role = await _rolesRepository.GetRoleAsync(roleId);

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

            var addedRole = await _rolesRepository.AddRoleAsync(newRole);
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

            response.Role = await _rolesRepository.UpdateRoleAsync(roleId, updatedRole);
            if(response.Role == null)            
                response.ErrorMessage = $"Role with Id {roleId} does not exist";
            
            return response;
        }

        public async Task<GetRoleResponse> SoftDeleteRoleAsync(string roleId)
        {
            var response = new GetRoleResponse();
            if (!Guid.TryParse(roleId, out _))
            {
                response.ErrorMessage = "Role Id is not Guid";
                return response;
            }

            var roleForDelete = await _rolesRepository.GetRoleAsync(roleId);
            if (roleForDelete == null)
            {
                response.ErrorMessage = $"Role with Id {roleId} does not exist";
                return response;
            }

            await _userRolesService.DeleteUserRolesByRoleIdAsync(roleId);
            await _roleRequirementsService.DeleteRoleRequirementsByRoleIdAsync(roleId);
            response.Role = await _rolesRepository.SoftDeleteRoleAsync(roleForDelete);

            return response;
        }
    }
}
