using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroZoo.Infrastructure.Models.Specialities
{
    /// <summary>
    /// Provides information about relation between zookeeper and animal type
    /// </summary>
    [Table("specialities")]
    [Index(nameof(ZookeeperId), nameof(AnimalTypeId), 
        Name = "IX_Speciality", IsUnique = true)]
    public class Speciality
    {
        /// <summary>
        /// Instance id
        /// </summary>
        //[Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Zookeeper's id
        /// </summary>
        [Column("zookeeperid")]
        public int ZookeeperId { get; set; }

        /// <summary>
        /// Animal type id
        /// </summary>
        [Column("animaltypeid")]
        public int AnimalTypeId { get; set; }
    }
}
