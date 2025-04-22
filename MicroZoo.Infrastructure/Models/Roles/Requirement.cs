using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroZoo.Infrastructure.Models.Roles
{
    /// <summary>
    /// Provides information about an action that can do a role
    /// </summary>
    [Table("Requirements")]
    public class Requirement
    {
        /// <summary>
        /// Identificator of requirement
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Description of requirement
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Allows to mark a record as deleted without actually removing it from the database
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Navigation property to the foreign key in RoleRequirements table
        /// </summary>
        public List<RoleRequirement>? RoleRequirements { get; set; }

        /// <summary>
        /// Assigns properties from the instance of <see cref="RequirementWithoutIdDto"/>
        /// to the instance of <see cref="Requirement"/>
        /// </summary>
        /// <param name="requirementWithoutIdDto"></param>
        public void SetValues(RequirementWithoutIdDto requirementWithoutIdDto)
        {
            Id = Guid.NewGuid();
            Name = requirementWithoutIdDto.Name;
        }
    }
}
