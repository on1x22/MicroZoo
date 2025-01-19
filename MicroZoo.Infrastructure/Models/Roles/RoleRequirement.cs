using System.ComponentModel.DataAnnotations.Schema;

namespace MicroZoo.Infrastructure.Models.Roles
{
    [Table("RoleRequirements")]
    public class RoleRequirement
    {
        public string? RoleId { get; set; }
        public Role? Role { get; set; }

        public Guid RequirementId { get; set; }
        public Requirement? Requirement { get; set; }
    }
}
