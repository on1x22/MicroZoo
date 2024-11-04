using MicroZoo.Infrastructure.Models.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroZoo.IdentityApi.SeedConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = "e78f1a28-5a42-496a-adb2-9e5f70b51fc8",
                    Name = "First visitor",
                    NormalizedName = "FIRST VISITOR",
                    Description = "The visitor role for the user"
                },
                new Role
                {
                    Id = "c28c85a9-80ca-4768-993d-80a9410606c5",
                    Name = "First admin",
                    NormalizedName = "FIRST ADMIN",
                    Description = "The admin role for the user"
                });
        }
    }
}
