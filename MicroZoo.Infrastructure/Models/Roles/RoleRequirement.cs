using System.ComponentModel.DataAnnotations.Schema;

namespace MicroZoo.Infrastructure.Models.Roles
{
    /// <summary>
    /// Links roles and requirements
    /// </summary>
    [Table("RoleRequirements")]
    public class RoleRequirement
    {
        /// <summary>
        /// Role identificator
        /// </summary>
        public string? RoleId { get; set; }

        /// <summary>
        /// Navigation property to the instance of <see cref="Role"/> class
        /// </summary>
        public Role? Role { get; set; }

        /// <summary>
        /// Requirement identificator
        /// </summary>
        public Guid RequirementId { get; set; }

        /// <summary>
        /// Navigation property to the instance of <see cref="Requirement"/> class
        /// </summary>
        public Requirement? Requirement { get; set; }
    }
}
