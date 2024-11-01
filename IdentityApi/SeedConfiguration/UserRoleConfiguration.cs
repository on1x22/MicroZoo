using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityApi.SeedConfiguration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = "081cb69e-d710-470f-9971-96fc9df25db8",
                    RoleId = "e78f1a28-5a42-496a-adb2-9e5f70b51fc8"
                },
                new IdentityUserRole<string>
                {
                    UserId = "870db52f-33c0-44e1-a861-0602a1a998ce",
                    RoleId = "c28c85a9-80ca-4768-993d-80a9410606c5"
                });
        }
    }
}
