using MicroZoo.Infrastructure.MassTransit.Responses.IdentityApi;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.Services
{
    public interface IRolesService
    {
        Task<GetRolesResponse> GetAllRolesAsync();
        Task<GetRoleResponse> GetRoleAsync(string roleId);
        Task<GetRoleResponse> AddRoleAsync(RoleWithoutIdDto roleWithoutIdDto);
        Task<GetRoleResponse> UpdateRoleAsync(string roleId, RoleWithoutIdDto roleWithoutIdDto);
        Task<GetRoleResponse> SoftDeleteRoleAsync(string roleId);
    }
}
