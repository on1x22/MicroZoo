using Microsoft.EntityFrameworkCore;
using MicroZoo.PersonsApi.Models;

namespace MicroZoo.PersonsApi.DbContexts
{
    public class PersonDbContext : DbContext
    {
        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}
