using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MicroZoo.Infrastructure.Models.Persons
{
    [Table("personapi")]
    public class Person
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("firstname")]
        public string FirstName { get; set; }
        [Column("lastname")]
        public string LastName { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("ismanager")]
        public bool IsManager { get; set; }
        [Column("managerid")]
        public int ManagerId { get; set; }
        [Column("deleted")]
        public bool Deleted { get; set; }
    }
}
