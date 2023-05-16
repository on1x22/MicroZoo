using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroZoo.PersonsApi.Models
{
    [Table("personapi")]
    public class ___Person
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("firstname")]
        public string firstName { get; set; }
        [Column("lastname")]
        public string lastName { get; set; }
        [Column("email")] 
        public string email { get; set; }
        [Column("ismanager")]
        public bool isManager { get; set; }
        [Column("managerid")] 
        public int managerId { get; set; }
    }
}
