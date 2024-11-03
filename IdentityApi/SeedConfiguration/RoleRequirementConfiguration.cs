using MicroZoo.IdentityApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroZoo.IdentityApi.SeedConfiguration
{
    public class RoleRequirementConfiguration : IEntityTypeConfiguration<RoleRequirement>
    {
        public void Configure(EntityTypeBuilder<RoleRequirement> builder)
        {
            builder.HasData(
                new RoleRequirement
                {
                    RoleId = "c28c85a9-80ca-4768-993d-80a9410606c5",
                    RequirementId = new Guid("b1de0641-d730-4207-84be-ff27d6477229")
                },
                new RoleRequirement
                {
                    RoleId = "c28c85a9-80ca-4768-993d-80a9410606c5",
                    RequirementId = new Guid("b0e4d6a8-b603-4404-8340-68ef6c27cbb2")
                },
                new RoleRequirement
                {
                    RoleId = "c28c85a9-80ca-4768-993d-80a9410606c5",
                    RequirementId = new Guid("def637f0-5d66-4f0f-b2f0-558883d6144e")
                },
                new RoleRequirement
                {
                    RoleId = "c28c85a9-80ca-4768-993d-80a9410606c5",
                    RequirementId = new Guid("bed6d2dc-e231-43e2-8ee7-553a8e3db5b0")
                });
        }
    }
}
