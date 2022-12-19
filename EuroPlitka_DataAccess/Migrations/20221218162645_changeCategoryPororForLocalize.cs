using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EuroPlitkaDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changeCategoryPororForLocalize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Categorys");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categorys",
                newName: "NameUa");

            migrationBuilder.AddColumn<string>(
                name: "NameEng",
                table: "Categorys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameEng",
                table: "Categorys");

            migrationBuilder.RenameColumn(
                name: "NameUa",
                table: "Categorys",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Categorys",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
