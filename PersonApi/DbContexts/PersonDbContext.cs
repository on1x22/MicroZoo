using Microsoft.EntityFrameworkCore;
using MicroZoo.Infrastructure.Models.Persons;

namespace MicroZoo.PersonsApi.DbContexts
{
    /// <summary>
    /// Context to connect Persons database
    /// </summary>
    public class PersonDbContext : DbContext
    {
        /// <summary>
        /// Initialize a new instance of <see cref="PersonDbContext"/> class
        /// </summary>
        /// <param name="options"></param>
        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet for Persons table in database
        /// </summary>
        public DbSet<Person> Persons { get; set; }
    }
}
