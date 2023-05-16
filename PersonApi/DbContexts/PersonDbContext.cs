using Microsoft.EntityFrameworkCore;
using MicroZoo.PersonsApi.Models;
using MicroZoo.Infrastructure.Models.Persons;

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
