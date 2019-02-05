using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelExpenses.Persistence.Migrations
{
    public partial class transaction_keyword_cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionKeyword_Transaction_TransactionId",
                schema: "app",
                table: "TransactionKeyword");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionKeyword_Transaction_TransactionId",
                schema: "app",
                table: "TransactionKeyword",
                column: "TransactionId",
                principalSchema: "app",
                principalTable: "Transaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionKeyword_Transaction_TransactionId",
                schema: "app",
                table: "TransactionKeyword");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionKeyword_Transaction_TransactionId",
                schema: "app",
                table: "TransactionKeyword",
                column: "TransactionId",
                principalSchema: "app",
                principalTable: "Transaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
