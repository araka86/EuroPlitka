using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EuroPlitka_DataAccess.Migrations
{
    public partial class initNewsModeluser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "News",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_CreatedByUserId",
                table: "News",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_News_AspNetUsers_CreatedByUserId",
                table: "News",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_AspNetUsers_CreatedByUserId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_CreatedByUserId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "News");
        }
    }
}
