using Microsoft.EntityFrameworkCore.Migrations;

namespace Coresite.Data.Migrations
{
    public partial class MenuItem_plus_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MenuItem",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "MenuItem");
        }
    }
}
