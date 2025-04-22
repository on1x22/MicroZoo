using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MicroZoo.Infrastructure.Models.Animals
{
    /// <summary>
    /// Provides information about a type of the animal
    /// </summary>
    [Table("animaltypes")]
    public class AnimalType
    {
        /// <summary>
        /// Id of animal type
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Animal type description
        /// </summary>
        [Column("description"), MaxLength(50), NotNull]
        public string? Description { get; set; }

        /// <summary>
        /// Allows to mark a record as deleted without actually removing it from the database
        /// </summary>
        [Column("deleted")]
        public bool Deleted { get; set; }

        /// <summary>
        /// Navigation property to the foreign key in Animals table
        /// </summary>
        public List<Animal>? Animals { get; set; }
    }
}
