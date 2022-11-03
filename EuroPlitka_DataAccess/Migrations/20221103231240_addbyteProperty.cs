using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EuroPlitka_DataAccess.Migrations
{
    public partial class addbyteProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "imagebyte",
                table: "Product",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imagebyte",
                table: "Product");
        }
    }
}
