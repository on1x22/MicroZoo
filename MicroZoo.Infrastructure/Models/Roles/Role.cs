using Microsoft.AspNetCore.Identity;

namespace MicroZoo.Infrastructure.Models.Roles
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }

        public List<RoleRequirement>? RoleRequirements { get; set; }

        public void Update(Role roleForUpdate)
        {
            Description = roleForUpdate.Description;
            Name = roleForUpdate.Name;
            NormalizedName = roleForUpdate.NormalizedName;
            ConcurrencyStamp = roleForUpdate.ConcurrencyStamp;
        }

        public void SetValues(RoleWithoutIdDto roleWithoutIdDto)
        {
            Id = Guid.NewGuid().ToString();
            Description = roleWithoutIdDto?.Description;
            Name = roleWithoutIdDto?.Name;
            NormalizedName = roleWithoutIdDto?.NormalizedName;
            ConcurrencyStamp = roleWithoutIdDto?.ConcurrencyStamp;
        }
    }
}
