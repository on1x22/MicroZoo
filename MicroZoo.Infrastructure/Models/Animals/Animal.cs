using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MicroZoo.Infrastructure.Models.Animals
{
    /// <summary>
    /// Provides information about an animal in zoo
    /// </summary>
    [Table("animals")]
    public class Animal
    {
        /// <summary>
        /// Animal id
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Name of animal
        /// </summary>
        [Column("name"), MaxLength(50)]
        public string? Name { get; set; }

        /// <summary>
        /// Link to a site with information about the animal
        /// </summary>
        [Column("link"), MaxLength(250)]
        public string? Link { get; set; }

        /// <summary>
        /// Id of type of the animal
        /// </summary>
        [Column("animaltypeid"), NotNull]
        public int AnimalTypeId { get; set; } // foreign key

        /// <summary>
        /// Allows to mark a record as deleted without actually removing it from the database
        /// </summary>
        [Column("deleted")]
        public bool Deleted { get; set; }

        /// <summary>
        /// Navigation property to the instance of <see cref="AnimalType"/> class
        /// </summary>
        public AnimalType? AnimalType { get; set; }
    }
}
