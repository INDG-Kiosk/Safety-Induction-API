using Microsoft.EntityFrameworkCore.Migrations;

namespace INSEE.KIOSK.API.Migrations
{
    public partial class sitename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "M_Site",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "M_Site");
        }
    }
}
