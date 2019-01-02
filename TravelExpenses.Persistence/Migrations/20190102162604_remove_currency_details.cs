using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelExpenses.Persistence.Migrations
{
    public partial class remove_currency_details : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currency_Users_UserId",
                schema: "app",
                table: "Currency");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Currency_CurrencyId",
                schema: "app",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_CurrencyId",
                schema: "app",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Currency_UserId",
                schema: "app",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                schema: "app",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "HomeCurrencyRatio",
                schema: "app",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "IsHomeCurrency",
                schema: "app",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "app",
                table: "Currency");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                schema: "app",
                table: "Location",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "HomeCurrencyRatio",
                schema: "app",
                table: "Currency",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsHomeCurrency",
                schema: "app",
                table: "Currency",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "app",
                table: "Currency",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Location_CurrencyId",
                schema: "app",
                table: "Location",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_UserId",
                schema: "app",
                table: "Currency",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currency_Users_UserId",
                schema: "app",
                table: "Currency",
                column: "UserId",
                principalSchema: "app",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Currency_CurrencyId",
                schema: "app",
                table: "Location",
                column: "CurrencyId",
                principalSchema: "app",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
