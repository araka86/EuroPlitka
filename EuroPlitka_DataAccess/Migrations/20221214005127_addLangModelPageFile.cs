using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EuroPlitkaDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addLangModelPageFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PagefilleId",
                table: "Resources",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Pagefilles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagefilles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resources_PagefilleId",
                table: "Resources",
                column: "PagefilleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Pagefilles_PagefilleId",
                table: "Resources",
                column: "PagefilleId",
                principalTable: "Pagefilles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Pagefilles_PagefilleId",
                table: "Resources");

            migrationBuilder.DropTable(
                name: "Pagefilles");

            migrationBuilder.DropIndex(
                name: "IX_Resources_PagefilleId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "PagefilleId",
                table: "Resources");
        }
    }
}
