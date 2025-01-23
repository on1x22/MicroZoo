using Microsoft.EntityFrameworkCore;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.DbContexts
{
    /// <summary>
    /// Context to connect Animals database
    /// </summary>
    public class AnimalDbContext : DbContext
    {
        /// <summary>
        /// Initialize a new instance of <see cref="AnimalDbContext"/> class
        /// </summary>
        /// <param name="options"></param>
        public AnimalDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// DbSet for Animals table in database
        /// </summary>
        public DbSet<Animal> Animals { get; set; }

        /// <summary>
        /// bSet for AnimalTypes table in database
        /// </summary>
        public DbSet<AnimalType> AnimalTypes { get; set; }

        /// <summary>
        /// Performs initial table setup
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>()
                .HasOne(t => t.AnimalType)
                .WithMany(t => t.Animals)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
