using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroZoo.IdentityApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedDeleteValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Requirements",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "AspNetRoles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c28c85a9-80ca-4768-993d-80a9410606c5",
                column: "Deleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e78f1a28-5a42-496a-adb2-9e5f70b51fc8",
                column: "Deleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "081cb69e-d710-470f-9971-96fc9df25db8",
                columns: new[] { "ConcurrencyStamp", "Deleted", "SecurityStamp" },
                values: new object[] { "eb0959bb-77c2-4718-bd2f-fafc90566969", false, "16fefc12-7a36-4aea-bacf-6dfba841f656" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "870db52f-33c0-44e1-a861-0602a1a998ce",
                columns: new[] { "ConcurrencyStamp", "Deleted", "SecurityStamp" },
                values: new object[] { "4463637b-6d5f-43c6-93c2-295348dd8471", false, "d82c594f-9973-4806-8452-0900cb5586f6" });

            migrationBuilder.UpdateData(
                table: "Requirements",
                keyColumn: "Id",
                keyValue: new Guid("b0e4d6a8-b603-4404-8340-68ef6c27cbb2"),
                column: "Deleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Requirements",
                keyColumn: "Id",
                keyValue: new Guid("b1de0641-d730-4207-84be-ff27d6477229"),
                column: "Deleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Requirements",
                keyColumn: "Id",
                keyValue: new Guid("bed6d2dc-e231-43e2-8ee7-553a8e3db5b0"),
                column: "Deleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Requirements",
                keyColumn: "Id",
                keyValue: new Guid("def637f0-5d66-4f0f-b2f0-558883d6144e"),
                column: "Deleted",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Requirements");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "AspNetRoles");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "081cb69e-d710-470f-9971-96fc9df25db8",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4e3ad060-02db-4b64-aa7d-3497b29da461", "6cd0e361-a33a-41a0-b094-b14fc48bf154" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "870db52f-33c0-44e1-a861-0602a1a998ce",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "eb202dbe-d562-4791-8a0f-eebacb57d9f5", "286b153c-6b85-491f-8a72-3613bcc709c5" });
        }
    }
}
