using Microsoft.EntityFrameworkCore.Migrations;

namespace WG_ModelEF.Migrations
{
    public partial class FifteenthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "URLImage",
                table: "Tenant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "URLTenant",
                table: "Tenant",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "URLImage",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "URLTenant",
                table: "Tenant");
        }
    }
}
