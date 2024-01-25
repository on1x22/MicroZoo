using Microsoft.EntityFrameworkCore;
using MicroZoo.Infrastructure.Models.Jobs;
using MicroZoo.Infrastructure.Models.Specialities;
using MicroZoo.ZookeepersApi.Models;

namespace MicroZoo.ZookeepersApi.DBContext
{
    public class ZookeeperDBContext : DbContext
    {
        public ZookeeperDBContext(DbContextOptions<ZookeeperDBContext> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>().Property(j => j.Id)
                .HasIdentityOptions(startValue: 1);
        }

        public DbSet<Zookeeper> Zookepeers { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Speciality> Specialities { get; set; }


    }
}
