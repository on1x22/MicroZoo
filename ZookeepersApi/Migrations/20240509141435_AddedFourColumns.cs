using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroZoo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedFourColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "createdat",
                table: "jobs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "deadlinetime",
                table: "jobs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "priority",
                table: "jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "report",
                table: "jobs",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdat",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "deadlinetime",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "priority",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "report",
                table: "jobs");
        }
    }
}
