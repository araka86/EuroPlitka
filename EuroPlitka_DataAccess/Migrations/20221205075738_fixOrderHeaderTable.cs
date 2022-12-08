using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EuroPlitka_DataAccess.Migrations
{
    public partial class fixOrderHeaderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingDate",
                table: "OrderHeader");

            migrationBuilder.RenameColumn(
                name: "countItem",
                table: "OrderHeader",
                newName: "СountItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "СountItem",
                table: "OrderHeader",
                newName: "countItem");

            migrationBuilder.AddColumn<DateTime>(
                name: "ShippingDate",
                table: "OrderHeader",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
