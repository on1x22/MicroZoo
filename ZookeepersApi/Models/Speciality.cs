using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroZoo.ZookeepersApi.Models
{
    [Table("specialities")]
    public class Speciality
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("zookeeperid")]
        public int ZookeeperId { get; set; }
        [Column("animaltypeid")]
        public int AnimalTypeId { get; set; }
    }
}
