using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MicroZoo.Infrastructure.Models.Animals
{
    [Table("animaltypes")]
    public class AnimalType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("description"), MaxLength(50), NotNull]
        public string Description { get; set; }

        public List<Animal> Animals { get; set; }
    }
}
