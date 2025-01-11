using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroZoo.Infrastructure.Models.Roles;

namespace MicroZoo.IdentityApi.SeedConfiguration
{
    public class RequirementConfiguration : IEntityTypeConfiguration<Requirement>
    {
        public void Configure(EntityTypeBuilder<Requirement> builder)
        {
            builder.HasData(
                new Requirement
                {
                    Id = new Guid("b1de0641-d730-4207-84be-ff27d6477229"),
                    Name = "IdentityApi.Read"
                },
                new Requirement
                {
                    Id = new Guid("b0e4d6a8-b603-4404-8340-68ef6c27cbb2"),
                    Name = "IdentityApi.Create"
                },
                new Requirement
                {
                    Id = new Guid("def637f0-5d66-4f0f-b2f0-558883d6144e"),
                    Name = "IdentityApi.Update"
                },
                new Requirement
                {
                    Id = new Guid("bed6d2dc-e231-43e2-8ee7-553a8e3db5b0"),
                    Name = "IdentityApi.Delete"
                });
        }
    }
}
