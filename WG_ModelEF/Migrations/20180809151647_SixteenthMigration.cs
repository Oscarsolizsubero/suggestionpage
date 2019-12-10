using Microsoft.EntityFrameworkCore.Migrations;

namespace WG_ModelEF.Migrations
{
    public partial class SixteenthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Validation",
                table: "User",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Validation",
                table: "User");
        }
    }
}
