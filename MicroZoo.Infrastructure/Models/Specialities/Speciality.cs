using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroZoo.Infrastructure.Models.Specialities
{
    [Table("specialities")]
    [Index(nameof(ZookeeperId), nameof(AnimalTypeId), 
        Name = "IX_Speciality", IsUnique = true)]
    public class Speciality
    {
        //[Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("zookeeperid")]
        public int ZookeeperId { get; set; }

        [Column("animaltypeid")]
        public int AnimalTypeId { get; set; }
    }
}
