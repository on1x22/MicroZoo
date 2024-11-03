using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroZoo.IdentityApi.Models
{
    [Table("Requirements")]
    public class Requirement
    {
        [Key]
        //[Column("Id")]
        public Guid Id { get; set; }

        //[Column("Name")]
        public string? Name { get; set; }

        public List<RoleRequirement>? RoleRequirements { get; set; }
    }
}
