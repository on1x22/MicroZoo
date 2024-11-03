using Microsoft.AspNetCore.Identity;

namespace MicroZoo.IdentityApi.Models
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }

        public List<RoleRequirement>? RoleRequirements { get; set; }
    }
}
