using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroZoo.ZookeeperCatalog.Models
{
    [Table("zookeeper")]
    public class Zookepeer
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
