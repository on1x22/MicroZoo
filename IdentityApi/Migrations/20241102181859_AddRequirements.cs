using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroZoo.IdentityApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRequirements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requirements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requirements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleRequirements",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    RequirementId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleRequirements", x => new { x.RoleId, x.RequirementId });
                    table.ForeignKey(
                        name: "FK_RoleRequirements_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleRequirements_Requirements_RequirementId",
                        column: x => x.RequirementId,
                        principalTable: "Requirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_RoleRequirements_RequirementId",
                table: "RoleRequirements",
                column: "RequirementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleRequirements");

            migrationBuilder.DropTable(
                name: "Requirements");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "081cb69e-d710-470f-9971-96fc9df25db8",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "80ac17ec-a372-4cbd-80ba-622e2e017635", "1d9720c9-5c90-4801-a0c9-00980863d8ad" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "870db52f-33c0-44e1-a861-0602a1a998ce",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bf9d19f8-62ed-47e6-a0b5-d7344ee563c6", "7f056917-0871-46d5-814c-8514acb52d81" });
        }
    }
}
