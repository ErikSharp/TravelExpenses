using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelExpenses.Persistence.Migrations
{
    public partial class add_location_to_cash_withdrawal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                schema: "app",
                table: "CashWithdrawal",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_CashWithdrawal_LocationId",
                schema: "app",
                table: "CashWithdrawal",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashWithdrawal_Location_LocationId",
                schema: "app",
                table: "CashWithdrawal",
                column: "LocationId",
                principalSchema: "app",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashWithdrawal_Location_LocationId",
                schema: "app",
                table: "CashWithdrawal");

            migrationBuilder.DropIndex(
                name: "IX_CashWithdrawal_LocationId",
                schema: "app",
                table: "CashWithdrawal");

            migrationBuilder.DropColumn(
                name: "LocationId",
                schema: "app",
                table: "CashWithdrawal");
        }
    }
}
