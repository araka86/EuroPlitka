using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EuroPlitka_DataAccess.Migrations
{
    public partial class AddPropNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LeftSideInfoPicture",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RightSideInfoPicture",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeftSideInfoPicture",
                table: "News");

            migrationBuilder.DropColumn(
                name: "RightSideInfoPicture",
                table: "News");
        }
    }
}
