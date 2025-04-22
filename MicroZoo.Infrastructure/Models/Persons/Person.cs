using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MicroZoo.Infrastructure.Models.Persons
{
    /// <summary>
    /// Provides information about a person
    /// </summary>
    [Table("personapi")]
    public class Person
    {
        /// <summary>
        /// Person identificator
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// First name of person
        /// </summary>
        [Column("firstname")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name of person
        /// </summary>
        [Column("lastname")]
        public string? LastName { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        [Column("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Is person manager or ordinary worker
        /// </summary>
        [Column("ismanager")]
        public bool IsManager { get; set; }

        /// <summary>
        /// Id of the manager to whom the worker reports
        /// </summary>
        [Column("managerid")]
        public int ManagerId { get; set; }

        /// <summary>
        /// Allows to mark a record as deleted without actually removing it from the database
        /// </summary>
        [Column("deleted")]
        public bool Deleted { get; set; }
    }
}
