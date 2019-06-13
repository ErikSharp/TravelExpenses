using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelExpenses.Persistence.Migrations
{
    public partial class remove_location_withdrawals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashWithdrawal_Location_LocationId",
                schema: "app",
                table: "CashWithdrawal");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                schema: "app",
                table: "CashWithdrawal",
                nullable: true,
                oldClrType: typeof(int));

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

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                schema: "app",
                table: "CashWithdrawal",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CashWithdrawal_Location_LocationId",
                schema: "app",
                table: "CashWithdrawal",
                column: "LocationId",
                principalSchema: "app",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
