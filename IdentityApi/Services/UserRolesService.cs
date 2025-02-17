﻿using Microsoft.AspNetCore.Identity;
using MicroZoo.IdentityApi.Models.Mappers;
using MicroZoo.IdentityApi.Repositories;
using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;

namespace MicroZoo.IdentityApi.Services
{
    public class UserRolesService : IUserRolesService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IUserRolesRepository _userRolesRepository;
        private readonly IRolesRepository _rolesRepository;

        public UserRolesService(IUsersRepository usersRepository,
                                IUserRolesRepository userRolesRepository,
                                IRolesRepository rolesRepository)
        {
            _usersRepository = usersRepository;
            _userRolesRepository = userRolesRepository;
            _rolesRepository = rolesRepository;
        }

        public async Task<GetUserWithRolesResponse> GetUserWithRolesAsync(string userId)
        {
            var response = new GetUserWithRolesResponse();
            if (!Guid.TryParse(userId, out _))
            {
                response.ErrorMessage = "User Id is not Guid";
                return response;
            }

            var selectedUser = await _usersRepository.GetUserAsync(userId);
            if (selectedUser == null)
            {
                response.ErrorMessage = $"User with Id {userId} does not exist";
                return response;
            }

            var rolesOfUser = await _userRolesRepository.GetRolesOfUserAsync(userId);

            var userWithRoles = UserUpdater.ConvertToUserWithRoles(selectedUser);
            userWithRoles.Roles = rolesOfUser;

            response.UserWithRoles = userWithRoles;
            return response;
        }
            
        public async Task<GetUserWithRolesResponse> UpdateUserWithRolesAsync(string userId, 
                                                                              List<string> roleIds)
        {
            var response = new GetUserWithRolesResponse();
            if (!Guid.TryParse(userId, out _))
            {
                response.ErrorMessage = "User Id is not Guid";
                return response;
            }

            foreach (var roleId in roleIds)
            {
                if (!Guid.TryParse(roleId, out _))
                {
                    response.ErrorMessage = "Not all role Ids are Guid";
                    return response;
                }
            }

            var areAllIdsExistInDb = _rolesRepository.CheckEntriesIsExistInDatabase(roleIds);
            if(!areAllIdsExistInDb)
            {
                response.ErrorMessage = "Invalid list of roles Ids";
                return response;
            }

            var selectedUser = await _usersRepository.GetUserAsync(userId);
            if (selectedUser == null)
            {
                response.ErrorMessage = $"User with Id {userId} does not exist";
                return response;
            }

            var isSuccessfullyDeleted = await _userRolesRepository
                .DeleteUserRolesByUserIdAsync(userId);

            if (!isSuccessfullyDeleted)
            {
                response.ErrorMessage = "Innser server error";
                return response;
            }

            var newUserRoles = new List<IdentityUserRole<string>>();
            foreach (var roleId in roleIds)
            {
                var userRole = new IdentityUserRole<string>()
                {
                    UserId = userId,
                    RoleId = roleId
                };
                newUserRoles.Add(userRole);
            }

            var isSuccessfullyAdded = await _userRolesRepository
                .AddUserRolesAsync(newUserRoles);

            if (!isSuccessfullyAdded)
            {
                response.ErrorMessage = "Innser server error";
                return response;
            }

            return await GetUserWithRolesAsync(userId);
        }

        public async Task<bool> DeleteUserRolesByUserIdAsync(string userId)
        {
            if (!Guid.TryParse(userId, out _))
                return false;

            var isSuccessfullyDeleted = await _userRolesRepository
                .DeleteUserRolesByUserIdAsync(userId);

            if(!isSuccessfullyDeleted)
                return false;

            return true;
        }

        public async Task<bool> DeleteUserRolesByRoleIdAsync(string roleId)
        {
            if (!Guid.TryParse(roleId, out _))
                return false;

            var isSuccessfullyDeleted = await _userRolesRepository
                .DeleteUserRolesByRoleIdAsync(roleId);

            if (!isSuccessfullyDeleted)
                return false;

            return true;
        }
    }
}
