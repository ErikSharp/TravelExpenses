using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelExpenses.Persistence.Migrations
{
    public partial class category_color_icon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Color",
                schema: "app",
                table: "Category",
                nullable: false,
                defaultValue: 6591981);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                schema: "app",
                table: "Category",
                maxLength: 40,
                nullable: false,
                defaultValue: "live_help");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                schema: "app",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Icon",
                schema: "app",
                table: "Category");
        }
    }
}
