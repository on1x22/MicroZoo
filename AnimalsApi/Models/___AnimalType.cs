using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.Models
{
    [Table("animaltypes")]
    public class ___AnimalType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("description"), MaxLength(50), NotNull]
        public string Description { get; set; }

        public List<Animal> Animals { get; set; }
    }
}
