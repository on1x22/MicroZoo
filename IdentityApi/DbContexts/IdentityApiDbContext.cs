using IdentityApi.Models;
using IdentityApi.SeedConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityApi.DbContexts
{
    public class IdentityApiDbContext : IdentityDbContext<User, Role, string>
    {
        public IdentityApiDbContext(DbContextOptions<IdentityApiDbContext> options)
            : base(options) 
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
        }
    }
}
