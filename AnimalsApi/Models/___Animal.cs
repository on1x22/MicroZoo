using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.Models
{
    [Table("animals")]
    public class ___Animal
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

        public AnimalType AnimalType { get; set; }
    }
}
