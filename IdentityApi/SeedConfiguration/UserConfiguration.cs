using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroZoo.Infrastructure.Models.Users;

namespace MicroZoo.IdentityApi.SeedConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = "081cb69e-d710-470f-9971-96fc9df25db8",
                    FirstName = "First",
                    LastName = "User",
                    NormalizedUserName = "FIRST USER"                    
                },
                new User
                {
                    Id = "870db52f-33c0-44e1-a861-0602a1a998ce",
                    FirstName = "First",
                    LastName = "Admin",
                    NormalizedUserName = "FIRST ADMIN"
                });
        }
    }
}
