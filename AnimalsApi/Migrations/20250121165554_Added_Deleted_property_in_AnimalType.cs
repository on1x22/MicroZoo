using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimalsApi.Migrations
{
    /// <inheritdoc />
    public partial class Added_Deleted_property_in_AnimalType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "deleted",
                table: "animaltypes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted",
                table: "animaltypes");
        }
    }
}
