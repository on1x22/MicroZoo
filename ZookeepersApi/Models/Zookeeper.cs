using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroZoo.Infrastructure.Models.Specialities
{
    [Table("zookeeper")]
    public class Zookeeper
    {
        [Column("name")]
        public string Name { get; set; }
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("specialities")]
        public List<string> Specialities { get; set; }
    }
}
