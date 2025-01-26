using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MicroZoo.Infrastructure.Models.Animals
{
    [Table("animals")]
    public class Animal
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name"), MaxLength(50)]
        public string Name { get; set; }
        [Column("link"), MaxLength(250)]
        public string Link { get; set; }
        [Column("animaltypeid"), NotNull]
        public int AnimalTypeId { get; set; } // foreign key
        [Column("deleted")]
        public bool Deleted { get; set; }

        public AnimalType AnimalType { get; set; }
    }
}
