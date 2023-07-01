using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroZoo.ZookeepersApi.Migrations
{
    /// <inheritdoc />
    public partial class Changeprimarykey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Speciality",
                table: "specialities",
                columns: new[] { "zookeeperid", "animaltypeid" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Speciality",
                table: "specialities");
        }
    }
}
