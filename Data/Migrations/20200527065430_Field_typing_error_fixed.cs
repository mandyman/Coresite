using Microsoft.EntityFrameworkCore.Migrations;

namespace Coresite.Data.Migrations
{
    public partial class Field_typing_error_fixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stringStreetNumber",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "StreetNumber",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StreetNumber",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "stringStreetNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
