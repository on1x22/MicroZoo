using Microsoft.EntityFrameworkCore;
using MicroZoo.ZookeepersApi.Models;

namespace MicroZoo.ZookeepersApi.DBContext
{
    public class ZookeeperDBContext : DbContext
    {
        public ZookeeperDBContext(DbContextOptions<ZookeeperDBContext> options) : base(options)
        { 
        }

        public DbSet<Zookeeper> Zookepeers { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Speciality> Specialities { get; set; }
    }
}
