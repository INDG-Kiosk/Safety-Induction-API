using Microsoft.EntityFrameworkCore.Migrations;

namespace INSEE.KIOSK.API.Migrations
{
    public partial class videolang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Video_SN",
                table: "M_Course",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Video_TA",
                table: "M_Course",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Video_SN",
                table: "M_Course");

            migrationBuilder.DropColumn(
                name: "Video_TA",
                table: "M_Course");
        }
    }
}
