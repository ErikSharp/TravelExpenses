using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelExpenses.Persistence.Migrations
{
    public partial class remove_user_from_country : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Country_Users_UserId",
                schema: "app",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Country_UserId",
                schema: "app",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Country_CountryName_UserId",
                schema: "app",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_Country_CountryName",
                schema: "app",
                table: "Country",
                column: "CountryName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Country_CountryName",
                schema: "app",
                table: "Country");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "app",
                table: "Country",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Country_UserId",
                schema: "app",
                table: "Country",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Country_CountryName_UserId",
                schema: "app",
                table: "Country",
                columns: new[] { "CountryName", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Country_Users_UserId",
                schema: "app",
                table: "Country",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
