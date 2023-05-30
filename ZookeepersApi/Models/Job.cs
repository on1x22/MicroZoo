using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MicroZoo.ZookeepersApi.Models
{
    [Table("jobs")]
    public class Job
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("zookeeperid")]
        public int ZookeeperId { get; set; }
        [Column("description"), MaxLength(150)]
        public string Description { get; set; }
        [Column("starttime"), NotNull]
        public DateTime StartTime { get; set; }
        [Column("finishtime")]
        public DateTime? FinishTime { get; set; }
    }
}
