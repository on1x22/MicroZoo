using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroZoo.Infrastructure.Models.Roles
{
    [Table("Requirements")]
    public class Requirement
    {
        [Key]
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public List<RoleRequirement>? RoleRequirements { get; set; }

        public void SetValues(RequirementWithoutIdDto requirementWithoutIdDto)
        {
            Id = Guid.NewGuid();
            Name = requirementWithoutIdDto.Name;
        }
    }
}
