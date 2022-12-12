using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EuroPlitka_DataAccess.Migrations
{
    public partial class addProperty_Delivery_in_OrderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Delivery",
                table: "OrderHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delivery",
                table: "OrderHeader");
        }
    }
}
