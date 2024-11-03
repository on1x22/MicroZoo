using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MicroZoo.IdentityApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedRequirementsAndRoleRequirements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "081cb69e-d710-470f-9971-96fc9df25db8",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "16c4d309-d432-42ac-9740-87c74d73ff90", "0ac65052-49c1-49b7-a0da-a31df5d28fb4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "870db52f-33c0-44e1-a861-0602a1a998ce",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "fedc8a69-60de-4407-a13f-4a28601411bb", "e4672842-8e89-429f-9efc-094a1bbac36f" });

            migrationBuilder.InsertData(
                table: "Requirements",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("b0e4d6a8-b603-4404-8340-68ef6c27cbb2"), "IdentityApi.Create" },
                    { new Guid("b1de0641-d730-4207-84be-ff27d6477229"), "IdentityApi.Read" },
                    { new Guid("bed6d2dc-e231-43e2-8ee7-553a8e3db5b0"), "IdentityApi.Delete" },
                    { new Guid("def637f0-5d66-4f0f-b2f0-558883d6144e"), "IdentityApi.Update" }
                });

            migrationBuilder.InsertData(
                table: "RoleRequirements",
                columns: new[] { "RequirementId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("b0e4d6a8-b603-4404-8340-68ef6c27cbb2"), "c28c85a9-80ca-4768-993d-80a9410606c5" },
                    { new Guid("b1de0641-d730-4207-84be-ff27d6477229"), "c28c85a9-80ca-4768-993d-80a9410606c5" },
                    { new Guid("bed6d2dc-e231-43e2-8ee7-553a8e3db5b0"), "c28c85a9-80ca-4768-993d-80a9410606c5" },
                    { new Guid("def637f0-5d66-4f0f-b2f0-558883d6144e"), "c28c85a9-80ca-4768-993d-80a9410606c5" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleRequirements",
                keyColumns: new[] { "RequirementId", "RoleId" },
                keyValues: new object[] { new Guid("b0e4d6a8-b603-4404-8340-68ef6c27cbb2"), "c28c85a9-80ca-4768-993d-80a9410606c5" });

            migrationBuilder.DeleteData(
                table: "RoleRequirements",
                keyColumns: new[] { "RequirementId", "RoleId" },
                keyValues: new object[] { new Guid("b1de0641-d730-4207-84be-ff27d6477229"), "c28c85a9-80ca-4768-993d-80a9410606c5" });

            migrationBuilder.DeleteData(
                table: "RoleRequirements",
                keyColumns: new[] { "RequirementId", "RoleId" },
                keyValues: new object[] { new Guid("bed6d2dc-e231-43e2-8ee7-553a8e3db5b0"), "c28c85a9-80ca-4768-993d-80a9410606c5" });

            migrationBuilder.DeleteData(
                table: "RoleRequirements",
                keyColumns: new[] { "RequirementId", "RoleId" },
                keyValues: new object[] { new Guid("def637f0-5d66-4f0f-b2f0-558883d6144e"), "c28c85a9-80ca-4768-993d-80a9410606c5" });

            migrationBuilder.DeleteData(
                table: "Requirements",
                keyColumn: "Id",
                keyValue: new Guid("b0e4d6a8-b603-4404-8340-68ef6c27cbb2"));

            migrationBuilder.DeleteData(
                table: "Requirements",
                keyColumn: "Id",
                keyValue: new Guid("b1de0641-d730-4207-84be-ff27d6477229"));

            migrationBuilder.DeleteData(
                table: "Requirements",
                keyColumn: "Id",
                keyValue: new Guid("bed6d2dc-e231-43e2-8ee7-553a8e3db5b0"));

            migrationBuilder.DeleteData(
                table: "Requirements",
                keyColumn: "Id",
                keyValue: new Guid("def637f0-5d66-4f0f-b2f0-558883d6144e"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "081cb69e-d710-470f-9971-96fc9df25db8",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "dad82446-9dbe-4e0e-8ed2-635e251434cd", "392e21f1-6f2c-4c59-b577-ff32403fbc3a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "870db52f-33c0-44e1-a861-0602a1a998ce",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "fd3299ea-4739-4a81-9905-90f27afaae1b", "11cd1235-5dbf-4cc0-b156-a094778a641a" });
        }
    }
}
