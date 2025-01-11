using MicroZoo.IdentityApi.SeedConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicroZoo.Infrastructure.Models.Users;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.DbContexts
{
    public class IdentityApiDbContext : IdentityDbContext<User, Role, string>
    {
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<RoleRequirement> RoleRequirements { get; set; }

        public IdentityApiDbContext(DbContextOptions<IdentityApiDbContext> options)
            : base(options) 
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
            builder.ApplyConfiguration(new RequirementConfiguration());
            builder.ApplyConfiguration(new RoleRequirementConfiguration());

            builder.Entity<RoleRequirement>()
                .HasKey(rr => new { rr.RoleId, rr.RequirementId });
        }
    }
}
