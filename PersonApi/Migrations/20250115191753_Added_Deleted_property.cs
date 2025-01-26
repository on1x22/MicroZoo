using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroZoo.PersonsApi.Migrations
{
    /// <inheritdoc />
    public partial class Added_Deleted_property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                table: "personapi",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted",
                table: "personapi");
        }
    }
}
