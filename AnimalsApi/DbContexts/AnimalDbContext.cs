using Microsoft.EntityFrameworkCore;
using MicroZoo.AnimalsApi.Models;
using MicroZoo.Infrastructure.Models.Animals;

namespace MicroZoo.AnimalsApi.DbContexts
{
    public class AnimalDbContext : DbContext
    {
        public AnimalDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Animal> Animals { get; set; }

        public DbSet<AnimalType> AnimalTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>()
                .HasOne(t => t.AnimalType)
                .WithMany(t => t.Animals)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
