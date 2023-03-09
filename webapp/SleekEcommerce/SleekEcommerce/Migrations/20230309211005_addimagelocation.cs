using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SleekEcommerce.Migrations
{
    public partial class addimagelocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageLocation",
                table: "Products",
                type: "varchar(500)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLocation",
                table: "Products");
        }
    }
}
