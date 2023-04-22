using Microsoft.EntityFrameworkCore;
using MicroZoo.ZookeeperCatalog.Models;

namespace MicroZoo.ZookeeperCatalog.DBContext
{
    public class ZookeeperDBContext : DbContext
    {
        public ZookeeperDBContext(DbContextOptions<ZookeeperDBContext> options) : base(options)
        { 
        }

        public DbSet<Zookepeer> Zookepeers { get; set; }

    }
}
