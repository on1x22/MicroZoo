using Microsoft.AspNetCore.Identity;

namespace MicroZoo.Infrastructure.Models.Roles
{
    /// <summary>
    /// Provides information about the role that can have a user
    /// </summary>
    public class Role : IdentityRole
    {
        /// <summary>
        /// Description of the role
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Allows to mark a record as deleted without actually removing it from the database
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Navigation property to the foreign key in RoleRequirements table
        /// </summary>
        public List<RoleRequirement>? RoleRequirements { get; set; }

        /// <summary>
        /// Copies properties from other instance of <see cref="Role"/>
        /// </summary>
        /// <param name="roleForUpdate"></param>
        public void Update(Role roleForUpdate)
        {
            Description = roleForUpdate.Description;
            Name = roleForUpdate.Name;
            NormalizedName = roleForUpdate.NormalizedName;
            ConcurrencyStamp = roleForUpdate.ConcurrencyStamp;
        }

        /// <summary>
        /// Assigns properties from the instance of <see cref="RoleWithoutIdDto"/>
        /// to the instance of <see cref="Role"/>
        /// </summary>
        public void SetValues(RoleWithoutIdDto roleWithoutIdDto)
        {
            Id = Guid.NewGuid().ToString();
            Description = roleWithoutIdDto?.Description;
            Name = roleWithoutIdDto?.Name;
            NormalizedName = roleWithoutIdDto?.NormalizedName;
            ConcurrencyStamp = roleWithoutIdDto?.ConcurrencyStamp;
        }

        /// <summary>
        /// Convert instane of <see cref="Role"/> class to the instance 
        /// of the <see cref="RoleWithRequirements"/> class
        /// </summary>
        /// <returns></returns>
        public RoleWithRequirements ConvertToRoleWithRequirements()
        {
            return new RoleWithRequirements
            {
                Id = Id,
                Description = Description,
                Name = Name,
                NormalizedName = NormalizedName,
                ConcurrencyStamp = ConcurrencyStamp                
            };
        }
    }
}
