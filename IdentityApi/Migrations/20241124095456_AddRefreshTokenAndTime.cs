using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroZoo.IdentityApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenAndTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "081cb69e-d710-470f-9971-96fc9df25db8",
                columns: new[] { "ConcurrencyStamp", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp" },
                values: new object[] { "4e3ad060-02db-4b64-aa7d-3497b29da461", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "6cd0e361-a33a-41a0-b094-b14fc48bf154" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "870db52f-33c0-44e1-a861-0602a1a998ce",
                columns: new[] { "ConcurrencyStamp", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp" },
                values: new object[] { "eb202dbe-d562-4791-8a0f-eebacb57d9f5", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "286b153c-6b85-491f-8a72-3613bcc709c5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers");

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
        }
    }
}
